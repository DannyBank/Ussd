using Dbank.Digisoft.Ussd.Data.Extensions;
using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.Data.Models.Hubtel;
using Dbank.Digisoft.Ussd.SDK.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Dbank.Digisoft.Engine.Payment.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase {
        private HubtelHelper _hubtelHelper;
        private PaystackHelper _paystackHelper;
        private PaymentDataHelper _paymentHelper;
        private ILogger<PaymentController> _logger;

        public PaymentController(HubtelHelper hubtelHelper, 
            ILogger<PaymentController> logger, PaymentDataHelper paymentHelper) {
            _hubtelHelper = hubtelHelper;
            _logger = logger;
            _paymentHelper = paymentHelper;
        }

        [HttpPost("hubtel/[action]")]
        public async Task<PaymentResponse?> RequestSingle([FromBody] PaymentRequestModel model) {
            try {
                if (!ModelState.IsValid) return null!;
                _ = model ?? throw new ArgumentNullException(nameof(model));

                var request = await _paymentHelper.CreatePayment(
                    model.Msisdn,
                    model.Amount,
                    model.Title,
                    model.Description,
                    DateTime.Now,
                    model.Msisdn.GenerateTransactionId());
                if (request == null) return null!;

                return await _hubtelHelper.RequestSingle(request, model.Msisdn);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(RequestSingle));
                return null;
            }
        }

        [HttpPost("paystack/[action]")]
        public async Task<PaymentResponse?> PaystackRequest([FromBody] PaymentRequestModel model) {
            try {
                if (!ModelState.IsValid) return null!;
                _ = model ?? throw new ArgumentNullException(nameof(model));


            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(PaystackRequest));
                return null;
            }
        }
    }
}
