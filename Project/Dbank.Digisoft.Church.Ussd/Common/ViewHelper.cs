using Dbank.Digisoft.Ussd.SDK.Session.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dbank.Digisoft.Church.Ussd.Abstractions;
using Dbank.Digisoft.Church.Ussd.Menus;
using System.Linq;

namespace Dbank.Digisoft.Church.Ussd.Common {
    public class ViewHelper: IViewHelper {
        public async Task<List<UssdMenuItem>> GetMenuFromList(
            UssdMenuItem input, SessionInfo sessionData, List<ChurchMenu> menuData, string handler, object data) {

            var menuItems = new List<UssdMenuItem>();
            menuItems.AddRange(menuData.Select(r =>
                        new UssdMenuItem {
                            Text = r.Text,
                            Data = data,
                            DataType = data.GetType(),
                            Position = r.Position
                        }));
            return menuItems;
        }
    }
}
