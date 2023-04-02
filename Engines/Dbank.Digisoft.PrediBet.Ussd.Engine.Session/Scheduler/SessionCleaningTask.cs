using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dbank.Digisoft.PrediBet.Engine.Session.Scheduler
{
    public class SessionCleaningTask : ScheduledProcessor
    {
        private readonly SessionDataHelper _dataHelper;
        private readonly ILogger<SessionCleaningTask> _logger;
        private List<Task> _runningTasks = new List<Task>();

        public SessionCleaningTask(IServiceScopeFactory serviceScopeFactory,
            SessionDataHelper dataHelper,
            ILogger<SessionCleaningTask> logger) : base(serviceScopeFactory)
        {
            _dataHelper = dataHelper;
            _logger = logger;
        }

        protected override string Schedule => "0 0 * * *";

        public override async Task ProcessInScope(IServiceProvider serviceProvider)
        {
            try
            {
                await Task.Run(async delegate { await ProcessCleanUp(); });
            }
            catch (Exception e)
            {
                //await _logger.LogError(e);
                _logger.LogCritical(e, $"Error Occured executing {nameof(ProcessInScope)}");
            }
        }

        #region Session Cleanup

        private async Task ProcessCleanUp()
        {
           

            try
            {
                var sessions = await _dataHelper.GetAbandonedSessions();
                var sesKeys = sessions.Select(k => (SessionId: k.SessionId, Msisdn: k.Msisdn, Date: k.StartTime, App: k.AppId, Stage: k.Stage));
                _runningTasks.Add(Task.Run(_dataHelper.ClearSessions));
                _runningTasks.Add(Task.Run(async () =>
                {
                    var abdSess = new HashSet<long>();
                    foreach (var sesKey in sesKeys)
                    {
                        abdSess.Add(sesKey.SessionId);
                       
                    }
                }));
                while (_runningTasks.Count > 0)
                {
                    _ = Task.Run(ClearDoneTasks);
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        private Task ClearDoneTasks()
        {
            var done = new List<Task>();
            foreach (var runingThread in _runningTasks)
            {
                if (runingThread.IsCompleted)
                {
                    done.Add(runingThread);
                }
            }

            if (done.Any()) _runningTasks.RemoveAll(t => done.Contains(t));
            done.Clear();
            return Task.CompletedTask;
        }

        #endregion Session Cleanup
    }
}