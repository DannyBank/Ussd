using Dbank.Digisoft.PrediBet.Api.Abstractions;
using Dbank.Digisoft.PrediBet.Api.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentClient _payClient;

        public PaymentController(IPaymentClient payClient)
        {
            _payClient = payClient;
        }

        [HttpPost("[action]")]
        public async Task<PaymentResponse> RequestPayment([FromBody] PaymentRequest request)
        {
            return await _payClient.RequestPayment(request);
        }

        [HttpGet("[action]")]
        public async Task<PaymentResponse> Callback()
        {
            return null!;
        }
    }
}
