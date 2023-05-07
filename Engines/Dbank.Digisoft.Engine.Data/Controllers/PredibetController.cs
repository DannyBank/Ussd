using Dbank.Digisoft.Ussd.Data.Models.PrediBet;
using Dbank.Digisoft.Ussd.SDK.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Dbank.Digisoft.Engine.Data.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PredibetController : ControllerBase {

        private readonly PrediBetDataHelper _dbHelper;
        private readonly ILogger<PredibetController> _logger;

        public PredibetController(PrediBetDataHelper dbHelper, ILogger<PredibetController> logger) {
            _dbHelper = dbHelper;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<List<Booking>?> Bookings() {
            try {
                return await _dbHelper.GetBookings();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(Bookings));
                return null;
            }
        }

        [HttpGet("booking/code/{code}")]
        public async Task<List<Booking>?> BookingByCode(string code) {
            try {
                return await _dbHelper.GetBookingByCode(code);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(BookingByCode));
                return null;
            }
        }

        [HttpGet("booking/id/{id}")]
        public async Task<Booking?> BookingById(long id) {
            try {
                return await _dbHelper.GetBookingById(id);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(BookingById));
                return null;
            }
        }
    }
}
