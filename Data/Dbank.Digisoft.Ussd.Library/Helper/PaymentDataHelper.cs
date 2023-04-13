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

        public async Task<PaymentRequest?> CreatePayment(string msisdn, decimal amount, 
            DateTime daterequested, long transId)
        {
            try
            {
                var input = new {
                    msisdn,
                    transactionId = transId,
                    amount,
                    date = daterequested,
                    status = "INIT"
                };
                using var connection = _db.CreateConnection("Payment");
                var payment = await connection.QuerySingleOrDefaultAsync<PaymentRequest>("createpayment",
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
