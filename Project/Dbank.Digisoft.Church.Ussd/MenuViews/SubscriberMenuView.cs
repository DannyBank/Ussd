using Dbank.Digisoft.Church.Ussd.Abstractions;
using Dbank.Digisoft.Church.Ussd.Menus;
using Dbank.Digisoft.Ussd.Data.Abstractions;
using Dbank.Digisoft.Ussd.Data.Clients;
using Dbank.Digisoft.Ussd.Data.Extensions;
using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.Data.Models.ChurchModels;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Models;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Church.Ussd.MenuViews {
    public class SubscriberMenuView : MenuView<SubscriberMenu> {
        private readonly ILogger<SubscriberMenuView> _logger;
        private readonly MenuData _menuData;
        private readonly AppSettings _appSettings;
        private readonly AppStrings _appStrings;
        private readonly IChurchClient _dbClient;
        private readonly IViewHelper _viewHelper;

        public SubscriberMenuView(ILogger<SubscriberMenuView> logger,
            IOptionsSnapshot<MenuData> menuData,
            IOptionsSnapshot<AppSettings> appSettings,
            IOptionsSnapshot<AppStrings> appStrings,
            ChurchClient db, IViewHelper viewHelper) {
            _logger = logger;
            _menuData = menuData.Value;
            _appSettings = appSettings.Value;
            _appStrings = appStrings.Value;
            _dbClient = db;
            _viewHelper = viewHelper;
        }

        [Handler("Index")]
        private async Task<MenuCollection> Index(UssdMenuItem input, SessionInfo sessionData = null) {            
            var selectedChurch = sessionData.GetSessionData<Digisoft.Ussd.Data.Models.ChurchModels.Church>(
                AppConstants.SELECTED_CHURCH);
            if (selectedChurch == null)
                return new(_appStrings.NoChurchWasSelected);

            var subscriberName = sessionData.CurrentInput.Trim();
            if (string.IsNullOrWhiteSpace(subscriberName))
                return new(_appStrings.InvalidInput) { RequiresInput = true };

            var subscriber = await _dbClient.RecordSubscriber(new Subscriber {
                Msisdn = sessionData.Msisdn,
                Name = subscriberName
            });
            if (subscriber is null)
                return new(_appStrings.RecordChurchSubscriberFailed);

            var result = await _dbClient.RecordSubscriberForChurch(
                new SubscriberChurchModel {
                    SubscriberName = subscriberName,
                    Msisdn = sessionData.Msisdn.ToIntMsisdn(IntCode.GHA),
                    ChurchId = selectedChurch.ChurchId,
                    ChurchName = selectedChurch.Name
                });
            if (result == null || result.Id == 0)
                return new(_appStrings.RecordChurchSubscriberFailed);

            var data = _menuData.ChurchMenu.Select(r => new UssdMenuItem {
                Data = r,
                DataType = r.GetType(),
                Position = r.Position,
                Text = r.Text
            });
            var menuItems = await _viewHelper.GetMenuFromList(input, sessionData, _menuData.ChurchMenu,
                "Index", data);

            return new(menuItems, _appStrings.SelectChurchAction);
        }
    }
}
