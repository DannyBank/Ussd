using Dbank.Digisoft.Ussd.SDK.Session.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Session.Models;

namespace Dbank.Digisoft.TranscriptDelivery.Ussd.Helpers {
    public class NavigationStack
    {
        private readonly ISessionServiceHelper _sessionHelper;
        private readonly MenuStore _menuStore;

        private const string NAV_STACK_KEY = "navigationStackKey";

        public NavigationStack(
            ISessionServiceHelper sessionHelper,
            MenuStore menuStore)
        {
            _sessionHelper = sessionHelper;
            _menuStore = menuStore;
        }

        public void PushMenu(UssdMenuItem item, SessionInfo session) 
        {
            if (item == null) return;
            if (_menuStore.GetMenuCollection(session)?.CanBackTo == false) return;
            var navList = session.GetSessionData<List<UssdMenuItem>>(NAV_STACK_KEY)
                ?? new List<UssdMenuItem>();
            navList.Add(item);
            _sessionHelper.StoreSessionData(session, NAV_STACK_KEY, navList);
        }

        public UssdMenuItem PopMenu(SessionInfo session)
        {
            var navList = session.GetSessionData<List<UssdMenuItem>>(NAV_STACK_KEY);
            if (navList == null || navList?.Count < 2) return null;
            var item = navList[navList.Count - 2];
            if (item == null) return null;
            navList.RemoveAt(navList.Count - 1);
            _sessionHelper.StoreSessionData(session, NAV_STACK_KEY, navList);
            return item;
        }

        //developers SHould always clear the stack once returning to the MainMenu
        public void ClearMenus(SessionInfo session)
        {
            _sessionHelper.StoreSessionData(session, NAV_STACK_KEY, new List<UssdMenuItem>());
        }
        
    }
}
