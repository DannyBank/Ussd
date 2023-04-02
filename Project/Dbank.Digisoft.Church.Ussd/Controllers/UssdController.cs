using Dbank.Digisoft.Church.Ussd.Helpers;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Extensions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Church.Ussd.Controllers {
    [Route("[controller]")]
    public class UssdController : Controller {
        private readonly AppSettings _appSettings;
        private readonly GatesHelper _gate;
        private readonly UssdHandler _handler;
        private readonly ILogger<UssdController> _logger;
        private readonly ISessionServiceHelper _sessionHelper;

        public UssdController(UssdHandler handler, GatesHelper gate, ISessionServiceHelper sessionHelper,
            IOptionsSnapshot<AppSettings> appSettings, ILogger<UssdController> logger) {
            _handler = handler;
            _gate = gate;
            _sessionHelper = sessionHelper;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> UnoEntry([FromRoute] UserInput data) {
            return new UnoContent(await Entry(data));
        }

        [HttpGet("[action]")]
        public async Task<UssdContent> Entry([FromRoute] UserInput data) {
            try {
                var (passed, failedMessage) = await _gate.CheckAllowed(data);

                return passed ? await ProcessRequest(data) : this.UssdContent(failedMessage);
            }
            catch (Exception ex) {
                var errorMeassage = "An Error Occured Processing Request";
                _logger.LogError(ex, $"{errorMeassage} for {{@RequestParameters}}", data);
                return this.UssdContent(errorMeassage);
            }
        }

        private async Task<SessionInfo> GetOrCreateSession(UserInput data, List<string> directInputs) {
            var session = await _sessionHelper.GetSession(data.Msisdn, data.DialogId, _appSettings.AppId);
            if (session == null) {
                var shortCode = GetDirectInputs(data.Input).FirstOrDefault();
                if (_gate.IsValidShortCode(shortCode)) {
                    session = await _sessionHelper.CreateSession(data.Msisdn, data.DialogId, _appSettings.AppId);
                    InitSessionData(session);
                }
            }

            _ = session ?? throw new NullReferenceException("SessionInfo is Null");

            session.CurrentInput = data.Input;
            return session;
        }

        private static void InitSessionData(SessionInfo? session) {
            if (session is not null) {
                session.StoreSessionData("initrunstatus", true);
                session.StoreSessionData(AppConstants.BUYFOROTHERS_SESSIONKEY, false);
                session.StoreSessionData("IsRedirectApp", false);
            }
        }

        private static List<string> GetDirectInputs(string input) {
            input = input.TrimStart('*').TrimEnd('#').Trim(' ');
            if (!string.IsNullOrWhiteSpace(input)) {
                var inputData = input.Split("*");
                return inputData.ToList();
            }

            return new();
        }

        private async Task<UssdContent> ProcessRequest(UserInput data) {
            var directInputs = GetDirectInputs(data.Input);
            UssdContent results = new();
            var session = await GetOrCreateSession(data, directInputs);
            session.StoreSessionData("DirectDialing", true);
            session.StoreSessionData("RequestType", data.RequestType);
            var checkInitCornvivaTrans = session.GetSessionData<bool>("IsComvivaTrans");
            if (checkInitCornvivaTrans) {
                directInputs.RemoveAt(1);
                data.Input = directInputs.First();
            }

            if (_appSettings.DirectDialEnable && directInputs.Count > 1) {
                session.StoreSessionData("initrunstatus", false);
                foreach (var directInput in directInputs) {
                    data.Input = directInput;
                    session.CurrentInput = directInput;
                    results = await UssdProccess(data, session);
                    if (!results.AllowInput) return results;
                }
            }
            else {
                session.StoreSessionData("DirectDialing", false);
                results = await UssdProccess(data, session);
            }

            if (!session.IsClosed)
                await _sessionHelper.Update(session);
            return results;
        }

        private async Task<UssdContent> UssdProccess(UserInput data, SessionInfo sess) {
            var output = await _handler.GetUssdMenu(sess, data.Input);
            return this.UssdContent(output);
        }
    }
}
