using Dbank.Digisoft.Church.Ussd.Abstractions;
using Dbank.Digisoft.Church.Ussd.Menus;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Models;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Church.Ussd.MenuViews {
    public class ChurchMenuView : MenuView<ChurchMenu> {
        private readonly ILogger<ChurchMenuView> _logger;
        private readonly MenuData _menuData;
        private readonly AppSettings _appSettings;
        private readonly AppStrings _appStrings;
        private readonly IViewHelper _viewHelper;

        public ChurchMenuView(ILogger<ChurchMenuView> logger,
            IOptionsSnapshot<MenuData> menuData,
            IOptionsSnapshot<AppSettings> appSettings,
            IOptionsSnapshot<AppStrings> appStrings,
            IViewHelper helper) {
            _logger = logger;
            _menuData = menuData.Value;
            _appSettings = appSettings.Value;
            _appStrings = appStrings.Value;
            _viewHelper = helper;
        }

        [Handler("HandleTithes")]
        private async Task<MenuCollection> HandleTithes(UssdMenuItem input, SessionInfo sessionData = null) {

            return await Task.FromResult(new MenuCollection("HandleTithes"));
        }


        [Handler("HandleGifts")]
        private async Task<MenuCollection> HandleGifts(UssdMenuItem input, SessionInfo sessionData = null) {

            return await Task.FromResult(new MenuCollection("HandleGifts"));
        }


        [Handler("HandleGivings")]
        private async Task<MenuCollection> HandleGivings(UssdMenuItem input, SessionInfo sessionData = null) {

            return await Task.FromResult(new MenuCollection("HandleGivings"));
        }


        [Handler("HandleDonations")]
        private async Task<MenuCollection> HandleDonations(UssdMenuItem input, SessionInfo sessionData = null) {

            return await Task.FromResult(new MenuCollection("HandleDonations"));
        }

    }
}
