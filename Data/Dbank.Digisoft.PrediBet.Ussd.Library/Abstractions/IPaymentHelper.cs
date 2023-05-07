using Dbank.Digisoft.PrediBet.Ussd.Data.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;

public interface IPaymentHelper
{
    public Task<string> ProcessPayment(SessionInfo sessionInfo, Product product, string currency = null, string otherNumber = null);
}