using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.Data.Models.ChurchModels;
using Dbank.Digisoft.Ussd.SDK.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Dbank.Digisoft.Engine.Data.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ChurchController : ControllerBase {
        private readonly ChurchDataHelper _dbHelper;
        private readonly ILogger<ChurchController> _logger;

        public ChurchController(ChurchDataHelper dbHelper, ILogger<ChurchController> logger) {
            _dbHelper = dbHelper;
            _logger = logger;
        }

        [HttpGet("all/by/subscriber/{msisdn}")]
        public async Task<List<SubscriberChurchModel>?> GetChurchesOfSubscriber(string msisdn) {
            try {
                if (!ModelState.IsValid) return null;
                return await _dbHelper.GetChurchesBySubscriber(msisdn);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(GetChurchesOfSubscriber));
                return null;
            }
        }

        [HttpGet("subscribers/churchid/{id}")]
        public async Task<List<Church>?> GetSubscribersByChurchId(int id) {
            try {
                if (!ModelState.IsValid) return null;
                return await _dbHelper.GetSubscribersByChurchId(id);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(GetSubscribersByChurchId));
                return null;
            }
        }

        [HttpGet("all")]
        public async Task<List<Church>?> GetAllChurches() {
            try {
                if (!ModelState.IsValid) return null;
                return await _dbHelper.GetChurches();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(GetAllChurches));
                return null;
            }
        }

        [HttpGet("subscriber/{msisdn}")]
        public async Task<Subscriber?> GetSubscriberByMsisdn(string msisdn) {
            try {
                if (!ModelState.IsValid) return null;
                return await _dbHelper.GetSubscriberByMsisdn(msisdn);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(GetSubscriberByMsisdn));
                return null;
            }
        }

        [HttpPost("record")]
        public async Task<SubscriberChurchModel?> RecordChurchSubscriber([FromBody] SubscriberChurchModel model) {
            try {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (!ModelState.IsValid) return null;
                return await _dbHelper.RecordChurchSubscriber(model);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(RecordChurchSubscriber));
                return null;
            }
        }

        [HttpPost("record/subscriber")]
        public async Task<Subscriber?> RecordSubscriber([FromBody] Subscriber model) {
            try {
                if (model == null) throw new ArgumentNullException(nameof(model));
                if (!ModelState.IsValid) return null;
                return await _dbHelper.RecordSubscriber(model);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(RecordSubscriber));
                return null;
            }
        }
    }
}
