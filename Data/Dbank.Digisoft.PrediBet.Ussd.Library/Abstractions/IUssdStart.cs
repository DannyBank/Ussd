using Dbank.Digisoft.PrediBet.Ussd.SDK.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;

public interface IUssdStart
{
    bool CheckTestModeAllowed(string msisdn, string dialogId);
    Task<bool> CheckServiceClassAsync(string input);
    Task<UssdMenu> ProcessStart(SessionInfo sessionInfo, string input);
    Task<UssdMenu> ProcessLevel1(SessionInfo sessionInfo, string input);
    Task<UssdMenu> ProcessLevel2(SessionInfo sessionInfo, string input);
    Task<UssdMenu> ProcessMenuSelection(SessionInfo sessionInfo, string input);
    Task<UssdMenu> ProcessLevel3(SessionInfo sessionInfo, string input);
        
}