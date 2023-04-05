using Dbank.Digisoft.Ussd;
using Dbank.Digisoft.Ussd.Menus;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Models;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Ussd.MenuViews {
    public class MainMenuView : MenuView<MainMenu> {
        private readonly ILogger<MainMenuView> _logger;
        private readonly MenuData _menuData;
        private readonly AppSettings _appSettings;
        private readonly AppStrings _appStrings;
        private readonly IApplicationDataHelper _appHelper;

        public MainMenuView(ILogger<MainMenuView> logger,
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
            var menuItems = new List<UssdMenuItem>();
            menuItems.AddRange(_menuData.StartMenu
                     .Select(r => new UssdMenuItem {
                        Text = r.Text,
                        Data = new MainMenu {
                            Handler = r.Handler,
                            Text = r.Text
                        },
                        DataType = typeof(MainMenu),
                        Position = r.Position
                     }));
            return await Task.FromResult(new MenuCollection(menuItems, _appStrings.WelcomeMessage));
        }

        [Handler("BookCodeEntryPrompt")]
        private async Task<MenuCollection> BookCodeEntryPrompt(UssdMenuItem input, SessionInfo sessionData = null) {
            StoreNextMenu(sessionData, "Index", "PredictionMenu");
            return await Task.FromResult(new MenuCollection(_appStrings.BookCodeEntryPrompt) {
                RequiresInput = true
            });
        }

        [Handler("DisplayBookedSets")]
        private async Task<MenuCollection> DisplayBookedSets(UssdMenuItem input, SessionInfo sessionData = null) {
            var menuItems = new List<UssdMenuItem>();
            var bookingList = await _appHelper.GetBookings();
            if (bookingList == null || bookingList.Count == 0)
                return new(_appStrings.NoPredictionsToday);

            var bookingCodes = bookingList.DistinctBy(r => r.Code);
            foreach (var booking in bookingCodes) {
                menuItems.Add(new() {
                    Data = new ViewPayMenu {
                        Text = booking.Code,
                        Handler = nameof(Index)
                    },
                    DataType = typeof(ViewPayMenu),
                    Text = booking.BookingName
                });
            }
            return new(menuItems, _appStrings.DisplayBookedSets);
        }

        private static void StoreNextMenu(SessionInfo session, string handler, string nextMenuType) {
            var nextMenuItem = new List<UssdMenuItem>
            {
                new UssdMenuItem
                {
                    DataType = typeof(PredictionMenu),
                    Data = new PredictionMenu
                    {
                        Handler = handler,
                        NextMenuType = nextMenuType
                    }
                }
            };
            var nextMenu = new MenuCollection(nextMenuItem);
            session.StoreSessionData(AppConstants.MENUITEMS, nextMenu);
        }
    }
}
