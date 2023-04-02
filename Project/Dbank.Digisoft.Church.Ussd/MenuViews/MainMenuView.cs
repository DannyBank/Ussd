using Dbank.Digisoft.PrediBet.Ussd.Menus;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Options;

namespace Dbank.Digisoft.Church.Ussd.MenuViews {
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
