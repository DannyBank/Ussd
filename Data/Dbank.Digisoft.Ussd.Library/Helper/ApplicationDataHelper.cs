using Dapper;
using Dbank.Digisoft.Ussd.Data.Abstractions;
using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.Data.Models.ChurchModels;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.SDK.Helper
{
    public class ApplicationDataHelper : IApplicationDataHelper
    {
        private readonly ILogger<ApplicationDataHelper> _logger;
        private readonly IDbHelper _db;

        public ApplicationDataHelper(ILogger<ApplicationDataHelper> logger, IDbHelper dbHelper)
        {
            _db = dbHelper;
            _logger = logger;
        }

        public static string GetFormattedSp(string sp) => $"\"{sp}\"";

        public async Task<List<SubscriberChurchModel>?> GetChurchesBySubscriber(string msisdn) {
            try {
                using var connection = _db.CreateConnection("Database");
                var list = await connection.QueryAsync<SubscriberChurchModel>(GetFormattedSp("get_churches_by_subscriber"),
                    new { p_msisdn = msisdn },
                    commandType: CommandType.StoredProcedure);
                return list.ToList();
            }
            catch (Exception e) {
                _logger.LogError(e, "Error Occured in {Method}", nameof(GetChurchesBySubscriber));
                return null;
            }
        }

        public async Task<List<Church>?> GetSubscribersByChurchId(int churchId) {
            try {
                using var connection = _db.CreateConnection("Database");
                var list = await connection.QueryAsync<Church>(GetFormattedSp("get_subscriber_church_by_id"),
                    new { p_church_id = churchId },
                    commandType: CommandType.StoredProcedure);
                return list.ToList();
            }
            catch (Exception e) {
                _logger.LogError(e, "Error Occured in {Method}", nameof(GetSubscribersByChurchId));
                return null;
            }
        }

        public Task<List<SmsOutbox>> GetSmsOutboxByBatch(int smsBatchCount) {
            throw new NotImplementedException();
        }

        public Task<SmsOutbox> SaveSmsOutbox(SmsOutbox request) {
            throw new NotImplementedException();
        }

        public Task UpdateSmsOutboxById(SmsOutbox r) {
            throw new NotImplementedException();
        }

        public async Task<SubscriberChurchModel?> RecordChurchSubscriber(
            SubscriberChurchModel subscriberModel) {
            try {
                using var connection = _db.CreateConnection("Database");
                var list = await connection.QuerySingleOrDefaultAsync<SubscriberChurchModel>(
                    GetFormattedSp("add_subscriber_church"),
                    new {
                        p_subscriber_name = subscriberModel.SubscriberName,
                        p_msisdn = subscriberModel.Msisdn,
                        p_church_id = subscriberModel.ChurchId,
                        p_date_created = DateTime.Now,
                        p_church_name = subscriberModel.ChurchName
                    },
                    commandType: CommandType.StoredProcedure);
                return list;
            }
            catch (Exception e) {
                _logger.LogError(e, "Error Occured in {Method}", nameof(RecordChurchSubscriber));
                return null;
            }
        }

        public async Task<List<Church>?> GetChurches() {
            try {
                using var connection = _db.CreateConnection("Database");
                var list = await connection.QueryAsync<Church>(GetFormattedSp("get_all_churches"),
                    commandType: CommandType.StoredProcedure);
                return list.ToList();
            }
            catch (Exception e) {
                _logger.LogError(e, "Error Occured in {Method}", nameof(GetChurches));
                return null;
            }
        }

        public async Task<Subscriber?> GetSubscriberByMsisdn(string msisdn) {
            try {
                using var connection = _db.CreateConnection("Database");
                var list = await connection.QuerySingleOrDefaultAsync<Subscriber>(GetFormattedSp("get_subscriber_by_msisdn"),
                    new { p_msisdn = msisdn },
                    commandType: CommandType.StoredProcedure);
                return list;
            }
            catch (Exception e) {
                _logger.LogError(e, "Error Occured in {Method}", nameof(GetSubscriberByMsisdn));
                return null;
            }
        }

        public async Task<Subscriber?> RecordSubscriber(Subscriber subscriber) {
            try {
                using var connection = _db.CreateConnection("Database");
                var list = await connection.QuerySingleOrDefaultAsync<Subscriber>(
                    GetFormattedSp("add_subscriber"),
                    new {
                        p_name = subscriber.Name,
                        p_msisdn = subscriber.Msisdn
                    },
                    commandType: CommandType.StoredProcedure);
                return list;
            }
            catch (Exception e) {
                _logger.LogError(e, "Error Occured in {Method}", nameof(RecordSubscriber));
                return null;
            }
        }
    }
}

