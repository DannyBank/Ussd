using Dapper;
using Dbank.Digisoft.Ussd.Data.Abstractions;
using Dbank.Digisoft.Ussd.Data.Models.PrediBet;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.SDK.Helper {
    public class PrediBetDataHelper
    {
        private readonly ILogger<PrediBetDataHelper> _logger;
        private readonly IDbHelper _db;
        private readonly string _constr;

        public PrediBetDataHelper(ILogger<PrediBetDataHelper> logger, IDbHelper dbHelper)
        {
            _db = dbHelper;
            _logger = logger;
            _constr = "PrediBet";
        }

        public static string GetFormattedSp(string sp) => $"\"{sp}\"";

        public async Task<List<Booking>?> GetBookingByCode(string text) {
            try {
                using var connection = _db.CreateConnection(_constr);
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
                using var connection = _db.CreateConnection(_constr);
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
                using var connection = _db.CreateConnection(_constr);
                var bookings = await connection.QueryAsync<Booking>(GetFormattedSp("getbookings"),
                    commandType: CommandType.StoredProcedure);
                return bookings.ToList();
            }
            catch (Exception e) {
                _logger.LogError(e, "Error Occured in {Method}", nameof(GetBookings));
                return null;
            }
        }
    }
}

