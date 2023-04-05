using Dapper;
using Dbank.Digisoft.Ussd.Data.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Threading.Tasks;
using System;

namespace Dbank.Digisoft.Ussd.SDK.Helper
{
    public class PaymentDataHelper
    {
        private readonly ILogger<PaymentDataHelper> _logger;
        private readonly DbHelper _db;
        private readonly AppOptions _options;

        public PaymentDataHelper(ILogger<PaymentDataHelper> logger, DbHelper dbHelper, IOptions<AppOptions> options)
        {
            _db = dbHelper;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<Payments?> SavePayment(Payments paymentMod)
        {
            try
            {
                using var connection = _db.CreateConnection("Database");
                var payment = await connection.QuerySingleOrDefaultAsync<Payments>("SavePayment",
                    new
                    {
                        orderid = paymentMod.OrderId,
                        amountpaid = paymentMod.AmountPaid,
                        balance = paymentMod.Balance,
                        date = paymentMod.Date,
                        status = paymentMod.Status
                    },
                    commandType: CommandType.StoredProcedure);

                return payment;

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error Occured Saving Inventory");
                return null;
            }
        }
    }
}
