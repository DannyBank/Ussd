using Dbank.Digisoft.Ussd.Data.Extensions;
using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.Data.Models.Hubtel;
using Dbank.Digisoft.Ussd.SDK.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Dbank.Digisoft.Engine.Payment.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase {
        private HubtelHelper _hubtelHelper;
        private PaymentDataHelper _paymentHelper;
        private ILogger<RequestController> _logger;

        public RequestController(HubtelHelper hubtelHelper, 
            ILogger<RequestController> logger, PaymentDataHelper paymentHelper) {
            _hubtelHelper = hubtelHelper;
            _logger = logger;
            _paymentHelper = paymentHelper;
        }

        [HttpPost("[action]")]
        public async Task<PaymentResponse?> Single([FromBody] PaymentRequestModel model) {
            try {
                if (!ModelState.IsValid) return null!;
                var request = await _paymentHelper.CreatePayment(
                    model.Msisdn, model.Amount, DateTime.Now, model.Msisdn.GenerateTransactionId());
                if (request == null) return null!;

                return await _hubtelHelper.RequestSingle(request, model.Msisdn);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(Single));
                return null;
            }
        }
    }
}
