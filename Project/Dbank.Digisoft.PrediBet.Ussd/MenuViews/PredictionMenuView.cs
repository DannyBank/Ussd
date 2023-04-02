using Dbank.Digisoft.PrediBet.Ussd;
using Dbank.Digisoft.PrediBet.Ussd.Data.Models;
using Dbank.Digisoft.PrediBet.Ussd.Menus;
using Dbank.Digisoft.PrediBet.Ussd.MenuViews;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.MenuViews {
    public class PredictionMenuView : MenuView<PredictionMenu> {
        private readonly ILogger<PredictionMenuView> _logger;
        private readonly MenuData _menuData;
        private readonly AppSettings _appSettings;
        private readonly AppStrings _appStrings;
        private readonly IApplicationDataHelper _appHelper;

        public PredictionMenuView(ILogger<PredictionMenuView> logger,
            IOptionsSnapshot<MenuData> menuData,
            IOptionsSnapshot<AppSettings> appSettings,
            IOptionsSnapshot<AppStrings> appStrings,
            IApplicationDataHelper db) {
            _logger = logger;
            _menuData = menuData.Value;
            _appSettings = appSettings.Value;
            _appStrings = appStrings.Value;
            _appHelper = db;
        }

        [Handler("Index")]
        private async Task<MenuCollection> Index(UssdMenuItem input, SessionInfo sessionData = null) {
            var enteredCode = sessionData.CurrentInput;
            var booking = await _appHelper.GetBookingByCode(enteredCode);
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
