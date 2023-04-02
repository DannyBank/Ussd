using Dbank.Digisoft.PrediBet.Ussd.Data.Extensions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using System;
using System.Linq;

namespace Dbank.Digisoft.PrediBet.Ussd.Helpers {
    public class MenuStore {
        private readonly UssdExceptionHandler _exceptHandlers;

        public MenuStore(
            UssdExceptionHandler exceptHandlers) {
            _exceptHandlers = exceptHandlers;
        }

        public void StoreMenuItems(SessionInfo session, MenuCollection menuItems) {
            if (menuItems.MenuItems.Count > 0)
                session.StoreSessionData(AppConstants.MENUITEMS, menuItems);
        }

        public MenuCollection? GetMenuCollection(SessionInfo session) {
            return session.GetSessionData<MenuCollection>(AppConstants.MENUITEMS);
        }

        public UssdMenuItem? GetSelectedMenuItem(SessionInfo session, int index) {
            try {
                var menulist = GetMenuCollection(session);
                if (menulist is not null) {
                    if (menulist.MenuItems.IsSingle()) return menulist.MenuItems.FirstOrDefault();
                    return menulist.MenuItems.ElementAtOrDefault(index);
                }

                return new();

            }
            catch (Exception e) {
                _exceptHandlers.ExceptionHandler(nameof(GetSelectedMenuItem), session, e);
                return default;
            }
        }
    }
}