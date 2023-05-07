using Dbank.Digisoft.Church.Ussd.Menus;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Church.Ussd.Abstractions {
    public interface IViewHelper {
        Task<List<UssdMenuItem>> GetMenuFromList(
            UssdMenuItem input, SessionInfo sessionData, List<ChurchMenu> menuData, string handler, object data);
    }
}
