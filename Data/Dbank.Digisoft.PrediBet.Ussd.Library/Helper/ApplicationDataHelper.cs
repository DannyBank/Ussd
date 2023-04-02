using Dapper;
using Dbank.Digisoft.PrediBet.Ussd.Data.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.Data.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Helper
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

        public async Task<List<Booking>?> GetBookingByCode(string text) {
            try {
                using var connection = _db.CreateConnection("Database");
                var booking = await connection.QueryAsync<Booking>(GetFormattedSp("getbookingbycode"),
                    new { code = text }, commandType: CommandType.StoredProcedure);
                return booking.ToList();
            }
            catch (Exception e) {
                _logger.LogError(e, "Error Occured in {Method}", nameof(GetBookingByCode));
                return null;
            }
        }

        public async Task<Booking?> GetBookingById(long bookingId) {
            try {
                using var connection = _db.CreateConnection("Database");
                var booking = await connection.QuerySingleOrDefaultAsync<Booking>(GetFormattedSp("getbookingbyid"),
                    new { bookingId }, commandType: CommandType.StoredProcedure);
                return booking;
            }
            catch (Exception e) {
                _logger.LogError(e, "Error Occured in {Method}", nameof(GetBookingById));
                return null;
            }
        }

        public async Task<List<Booking>?> GetBookings() {
            try {
                using var connection = _db.CreateConnection("Database");
                var bookings = await connection.QueryAsync<Booking>(GetFormattedSp("getbookings"),
                    commandType: CommandType.StoredProcedure);
                return bookings.ToList();
            }
            catch (Exception e) {
                _logger.LogError(e, "Error Occured in {Method}", nameof(GetBookings));
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
    }
}

