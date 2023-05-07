using Dapper;
using Dbank.Digisoft.Ussd.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Ussd.SDK.Helper {
    public class PaymentDataHelper
    {
        private readonly ILogger<PaymentDataHelper> _logger;
        private readonly DbHelper _db;

        public PaymentDataHelper(ILogger<PaymentDataHelper> logger, DbHelper dbHelper)
        {
            _db = dbHelper;
            _logger = logger;
        }

        public async Task<PaymentRequest?> CreatePayment(
            string msisdn, 
            decimal amount, 
            string title,
            string description,
            DateTime daterequested, 
            long transId)
        {
            try
            {
                var input = new {
                    p_msisdn = msisdn,
                    p_requesttitle = title,
                    p_transactionid = transId,
                    p_amount = amount,
                    p_requestdate = daterequested,
                    p_status = "INIT",
                    p_requestdescription = description
                };
                using var connection = _db.CreateConnection("Payment");
                var payment = await connection.QuerySingleOrDefaultAsync<PaymentRequest>("create_payment_request",
                    input,
                    commandType: CommandType.StoredProcedure);
                return payment;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured creating payment");
                return null;
            }
        }

        public async Task<PaymentRequest?> UpdatePayment(PaymentRequest payModel)
        {
            try
            {
                using var connection = _db.CreateConnection("Payment");
                var payment = await connection.QuerySingleOrDefaultAsync<PaymentRequest>("updatepayment",
                    new
                    {
                        transactionId = payModel.TransactionId,
                        status = "PND"
                    },
                    commandType: CommandType.StoredProcedure);
                return payment;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured creating payment with data {paymodel}", payModel);
                return null;
            }
        }
    }
}
