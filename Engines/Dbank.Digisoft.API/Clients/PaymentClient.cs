using Dbank.Digisoft.Api.Abstractions;
using Dbank.Digisoft.Api.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Api.Clients
{
	public class PaymentClient : IPaymentClient
    {
		private readonly ILogger<PaymentClient> _logger;
		private readonly IHubtelService _hubtel;

		public PaymentClient(ILogger<PaymentClient> logger, IHubtelService hubtelService)
		{
			_logger = logger;
			_hubtel = hubtelService;
		}

        public async Task<PaymentResponse> RequestPayment(PaymentRequest payload)
        {
			try
			{
				return await _hubtel.RequestPayment(payload);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred processing {Method} in {Class}", 
					nameof(RequestPayment), nameof(PaymentClient));
				return new();
			}
        }
    }
}
