using Dbank.Digisoft.PrediBet.Ussd;
using Dbank.Digisoft.PrediBet.Ussd.MenuViews;
using Dbank.Digisoft.Ussd.Data.Abstractions;
using Dbank.Digisoft.Ussd.Menus;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.MenuViews
{
    public class PredictionMenuView : MenuView<PredictionMenu> {
        private readonly ILogger<PredictionMenuView> _logger;
        private readonly MenuData _menuData;
        private readonly AppSettings _appSettings;
        private readonly AppStrings _appStrings;
        private readonly IPrediBetClient _dbClient;

        public PredictionMenuView(ILogger<PredictionMenuView> logger,
            IOptionsSnapshot<MenuData> menuData,
            IOptionsSnapshot<AppSettings> appSettings,
            IOptionsSnapshot<AppStrings> appStrings,
            IPrediBetClient db) {
            _logger = logger;
            _menuData = menuData.Value;
            _appSettings = appSettings.Value;
            _appStrings = appStrings.Value;
            _dbClient = db;
        }

        [Handler("Index")]
        private async Task<MenuCollection> Index(UssdMenuItem input, SessionInfo sessionData = null) {
            var enteredCode = sessionData.CurrentInput;
            var booking = await _dbClient.GetBookingsByCode(enteredCode);
            if (booking == null || booking.Count == 0)
                return new("No booking content was found for this code") { 
                    RequiresInput = true,
                    CanBackFrom = true
                };

            var menuItems = new List<UssdMenuItem>();
            menuItems.AddRange(booking.Select(r => new UssdMenuItem() {
                Data = new ViewPayMenu {
                    Text = r.Code,
                    Handler = nameof(Index)
                },
                DataType = typeof(ViewPayMenu),
                Text = r.ToString()
            }));
            menuItems.Add(new UssdMenuItem {
                Data = new ViewPayMenu {
                    Text = "Pay",
                    Handler = "PaymentPrompt"
                },
                DataType = typeof(ViewPayMenu),
                Text = "Pay"
            });
            return new(menuItems, _appStrings.SelectToUpdate);
        }
    }
}
