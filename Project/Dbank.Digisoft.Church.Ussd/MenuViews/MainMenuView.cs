using Dbank.Digisoft.Church.Ussd.Abstractions;
using Dbank.Digisoft.Church.Ussd.Menus;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Models;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartFormat;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Church.Ussd.MenuViews {
    public class MainMenuView : MenuView<MainMenu> {
        private readonly ILogger<MainMenuView> _logger;
        private readonly MenuData _menuData;
        private readonly AppSettings _appSettings;
        private readonly AppStrings _appStrings;
        private readonly IApplicationDataHelper _appHelper;
        private readonly IViewHelper _viewHelper;

        public MainMenuView(ILogger<MainMenuView> logger,
            IOptionsSnapshot<MenuData> menuData,
            IOptionsSnapshot<AppSettings> appSettings,
            IOptionsSnapshot<AppStrings> appStrings,
            IApplicationDataHelper db, IViewHelper helper) {
            _logger = logger;
            _menuData = menuData.Value;
            _appSettings = appSettings.Value;
            _appStrings = appStrings.Value;
            _appHelper = db;
            _viewHelper = helper;
        }

        [Handler("Index")]
        private async Task<MenuCollection> Index(UssdMenuItem input, SessionInfo sessionData = null) {
            var subscriberChurches = await _appHelper.GetChurchesBySubscriber(sessionData.Msisdn);
            if (subscriberChurches.Count == 1) {
                var subscriber = await _appHelper.GetSubscriberByMsisdn(sessionData.Msisdn);
                var churchName = subscriberChurches.FirstOrDefault()?.ChurchName ?? "Church name missing";

                var data = _menuData.ChurchMenu.Select(r => new UssdMenuItem {
                    Data = r,
                    DataType = r.GetType(),
                    Position = r.Position,
                    Text = r.Text
                });
                var menuItems = await _viewHelper.GetMenuFromList(input, sessionData, _menuData.ChurchMenu,
                    "Index", data);
                var header = Smart.Format(_appStrings.WelcomeSubscriberToChurch, new { ChurchName = churchName });
                return new(menuItems, Smart.Format(header, subscriber.Name, churchName));
            }
            else {
                var churchList = await _appHelper.GetChurches();
                if (!churchList.Any(r => r.ChurchId > 0)) {
                    _logger.LogInformation("Churches have not been uploaded yet");
                    return new(_appStrings.GenericError);
                }

                var menuItems = new List<UssdMenuItem>();
                churchList.ForEach(r => {
                    menuItems.Add(
                        new UssdMenuItem {
                            Text = r.Name,
                            Data = new MainMenu {
                                Handler = nameof(CheckSubscriberChurchExists),
                                Text = r.Name,
                                SelectedChurch = r
                            },
                            DataType = typeof(MainMenu),
                            Position = r.ChurchId
                        });
                });
                return await Task.FromResult(new MenuCollection(menuItems, _appStrings.WelcomeMessage));
            }            
        }

        [Handler("CheckSubscriberChurchExists")]
        private async Task<MenuCollection> CheckSubscriberChurchExists(UssdMenuItem input, SessionInfo sessionData = null) {
            var menuData = input.Data as MainMenu;
            if (menuData == null) return new(_appStrings.GenericError);

            sessionData.StoreSessionData(AppConstants.SELECTED_CHURCH, menuData.SelectedChurch);
            StoreNextMenu(sessionData, "Index", "SubscriberMenu");
            return await Task.FromResult(new MenuCollection(_appStrings.EnterNamePrompt) {
                RequiresInput = true,
                CanBackFrom = true
            });
        }

        private static void StoreNextMenu(SessionInfo session, string handler, string nextMenuType) {
            var nextMenuItem = new List<UssdMenuItem>
            {
                new UssdMenuItem
                {
                    DataType = typeof(SubscriberMenu),
                    Data = new SubscriberMenu
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
