using Dbank.Digisoft.Ussd.Menus;
using Dbank.Digisoft.Ussd;
using Dbank.Digisoft.Ussd.SDK.Handlers;
using Dbank.Digisoft.Ussd.SDK.Models;
using Dbank.Digisoft.Ussd.SDK.Session.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Ussd.Helpers {
    public class UssdHandler {
        private readonly AppSettings _appSettings;
        private readonly AppStrings _appStrings;
        private readonly UssdExceptionHandler _exceptHandler;
        private readonly MenuData _menuData;
        private readonly MenuHandler _menuHandler;
        private readonly MenuStore _menuStore;
        private readonly NavigationStack _navStack;
        private readonly ISessionServiceHelper _sessionHelper;


        public UssdHandler(IOptionsSnapshot<AppSettings> appSettings, ISessionServiceHelper sessionHelper,
            MenuStore menuStore, IOptionsSnapshot<AppStrings> appStrings, UssdExceptionHandler exceptHandler,
            MenuHandler menuHandler, NavigationStack navStack, IOptionsSnapshot<MenuData> menuData) {
            _sessionHelper = sessionHelper;
            _exceptHandler = exceptHandler;
            _menuHandler = menuHandler;
            _appSettings = appSettings.Value;
            _appStrings = appStrings.Value;
            _menuStore = menuStore;
            _navStack = navStack;
            _menuData = menuData.Value;
        }

        public async Task<UssdMenu> GetUssdMenu(SessionInfo session, string input) {
            try {
                if (IsAllowedInput(session, input) || IsDirectDialing(session))
                    return !IsPaginationInput(input) || IsRedirectsApp(session)
                        ? await ProcessOptionOrData(session, input)
                        : await ProcessPagination(session, input);
                return new() { Header = _appStrings.InvalidInput };
            }
            catch (Exception e) {
                return await _exceptHandler.ExceptionHandler(nameof(GetUssdMenu), session, e);
            }
        }

        private async Task<UssdMenu> ProcessPagination(SessionInfo session, string input) {
            var menuCollection = _menuStore.GetMenuCollection(session);
            return await GetNextOrPrevious(session, menuCollection, input);
        }

        private async Task<UssdMenu> ProcessOptionOrData(SessionInfo session, string input) {
            var selected = GetFirstOrSelectedMenuItem(session, input);
            _ = selected ?? throw new NullReferenceException("Selected UssdMenuItem cannot be null");
            _navStack.PushMenu(selected, session);
            if (!IsInitialRun(session)) MovetoNextStage(session);
            var menuCollection = await _menuHandler.ProcessMenu(session, selected);
            _menuStore.StoreMenuItems(session, menuCollection);

            return FirstPage(session, menuCollection);
        }

        private UssdMenuItem? GetFirstOrSelectedMenuItem(SessionInfo session, string input) {
            //First Request
            if (IsInitialRun(session)) {
                session.StoreSessionData("initrunstatus", false);
                return new() {
                    DataType = typeof(MainMenu),
                    Data = new MainMenu { Handler = "Index" }
                };
            }
            //Other Requests

            //OffMenu Direct Dials
            if (IsDirectDialing(session) && int.TryParse(input, out var selector)) {
                var item = GetOffMenuDirectDialItem(selector);
                if (item is not null) return item;
            }

            var index = GetMenuCollectionIndex(session, input);
            return _menuStore.GetSelectedMenuItem(session, index);
        }

        private bool IsDirectDialing(SessionInfo session) {
            var directDialing = session.GetSessionData<bool>("DirectDialing");
            if (directDialing)
                session.StoreSessionData("initrunstatus", false);
            return directDialing && _menuData.DirectDialMenu?.Any() == true &&
                   _appSettings.OffMenuDirectDialEnabled;
        }

        private UssdMenuItem? GetOffMenuDirectDialItem(int selector) {
            var item = _menuData.DirectDialMenu
                .Where(c => c.Active)
                .FirstOrDefault(d => d.ProductCodes.Any(r => r == selector));

            var emptyDirectDial = new DirectDialMenu() {
                Handler = "DisplayInvalidOption"
            };

            return item is null
                ? new UssdMenuItem {
                    DataType = emptyDirectDial.GetType(),
                    Data = emptyDirectDial
                }
                : new UssdMenuItem {
                    DataType = item.GetType(),
                    Data = item
                };
        }

        private static bool IsInitialRun(SessionInfo session) {
            return session.GetSessionData<bool>("initrunstatus");
        }

        private static bool IsRedirectsApp(SessionInfo session) {
            return session.GetSessionData<bool>("IsRedirectApp");
        }

        private UssdMenu FirstPage(SessionInfo session, MenuCollection menuCollection) {
            session.Page = 1;
            return MovePage(session, menuCollection);
        }

        private UssdMenu NextPage(SessionInfo session, MenuCollection menuCollection) {
            ++session.Page;
            return MovePage(session, menuCollection);
        }

        private UssdMenu PreviousPage(SessionInfo session, MenuCollection menuCollection) {
            session.Page = session.Page <= 1 ? 1 : --session.Page;
            return MovePage(session, menuCollection);
        }

        private static void MovetoNextStage(SessionInfo session) {
            ++session.Stage;
            session.Page = 1;
        }

        private static void MovetoPreviousStage(SessionInfo session) {
            if (session.Stage > SessionStage.Start) --session.Stage;
            session.Page = 1;
        }

        private async Task<UssdMenu> GetNextOrPrevious(SessionInfo session, MenuCollection menuCollection, string input) {
            if (input == _appSettings.NextPageCharacter) return NextPage(session, menuCollection);
            if (input == _appSettings.BackPageCharacter && session.Page == 1)
                return await PreviousMenu(session);
            if (input == _appSettings.BackPageCharacter) return PreviousPage(session, menuCollection);
            return new() {
                Header = _appStrings.ErrorOutput
            };
        }

        private async Task<UssdMenu> PreviousMenu(SessionInfo session) {
            var menuItem = _navStack.PopMenu(session);
            var prevMenuCollection = menuItem is null ? new(_appStrings.GenericError) : await _menuHandler.ProcessMenu(session, menuItem);
            _menuStore.StoreMenuItems(session, prevMenuCollection);
            MovetoPreviousStage(session);
            return FirstPage(session, prevMenuCollection);
        }

        private UssdMenu CreateUssdMenu(MenuCollection menuCollection, string seperator = ")",
            bool hasNext = false, bool hasBack = false) {
            var menustrings = menuCollection.MenuItems
                .OrderBy(c => c.Position)
                .Select(d => d.Text ?? string.Empty);

            return new(menustrings, hasNext, hasBack, menuCollection.RequiresInput) {
                Header = menuCollection.Header,
                Seperator = seperator,
                ExpectInput = menuCollection.RequiresInput,
                NextCharacter = _appSettings.NextPageCharacter,
                BackCharacter = _appSettings.BackPageCharacter
            };
        }

        private UssdMenu MovePage(SessionInfo session, MenuCollection menuCollection) {
            List<UssdMenuItem> pageItems;
            bool hasPrev;
            bool hasNext;

            var itemCount = _appSettings.MaxPageAtStage((int)session.Stage);
            var lowerBound = itemCount * (session.Page - 1);
            var upperBound = itemCount * session.Page;

            if (menuCollection.MenuItems.Count > upperBound) {
                pageItems = menuCollection.MenuItems.GetRange(lowerBound, upperBound);
                hasNext = true;
                hasPrev = lowerBound > 0 || CanGotoPreviousMenu(session, menuCollection);
            }
            else {
                upperBound = menuCollection.MenuItems.Count - lowerBound;
                pageItems = menuCollection.MenuItems.GetRange(lowerBound, upperBound);
                hasNext = false;
                hasPrev = session.Page > 1 || CanGotoPreviousMenu(session, menuCollection);
            }

            var pageCollection = new MenuCollection(pageItems, menuCollection.Header) {
                RequiresInput = menuCollection.RequiresInput
            };

            StoreAllowedInput(session, pageCollection, hasPrev, hasNext);

            if (!menuCollection.RequiresInput) //Close Session
                _ = Task.Run(() => _sessionHelper.CloseSession(session));

            return CreateUssdMenu(pageCollection,
                string.IsNullOrEmpty(_appSettings.MenuSeparator) ? ")" : _appSettings.MenuSeparator,
                hasNext, hasPrev);
        }

        private bool CanGotoPreviousMenu(SessionInfo session, MenuCollection menuCollection) {
            return session.Stage > SessionStage.Level1
                   && menuCollection.RequiresInput
                   && menuCollection.CanBackFrom
                   && _appSettings.CanGotoPreviousMenu;
        }

        private void StoreAllowedInput(SessionInfo session, MenuCollection menuCollection, bool hasPrev, bool hasNext) {
            var allowed = new List<string>();
            if (menuCollection.MenuItems.Count > 0 && menuCollection.RequiresInput)
                allowed = GenerateAllowedInput(menuCollection.MenuItems.Count, hasNext, hasPrev);
            //Add OffMenu Direct Dial inputs
            AddOffMenuDirectDial(allowed, session);
            session.StoreSessionData(AppConstants.ALLOWEDINPUT, allowed);
        }

        private void AddOffMenuDirectDial(List<string> allowed, SessionInfo session) {
            var isDirectDialing = session.GetSessionData<bool>("DirectDialing");
            if (_appSettings.OffMenuDirectDialEnabled
                && _menuData.DirectDialMenu?.Any() == true
                && isDirectDialing)
                allowed.AddRange(_menuData.DirectDialMenu
                    .Where(c => c.Active)
                    .Select(c => c.Position.ToString()));
        }

        private int GetMenuCollectionIndex(SessionInfo session, string input) {
            var allAllowed = GetAllowedInputs(session);
            if (!allAllowed?.Any() == true) return 0;
            var inputAsInt = Convert.ToInt32(input);
            return inputAsInt + (session.Page - 1) * _appSettings.MaxPageAtStage((int)session.Stage) - 1;
        }

        private static string[]? GetAllowedInputs(SessionInfo session) {
            return session.GetSessionData<string[]>(AppConstants.ALLOWEDINPUT);
        }

        private static bool IsAllowedInput(SessionInfo session, string? input = null) {
            var allAllowed = GetAllowedInputs(session);
            return allAllowed == null || !allAllowed.Any() || allAllowed.Contains(input ?? session.CurrentInput);
        }

        private bool IsPaginationInput(string input) {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            return input == _appSettings.NextPageCharacter || input == _appSettings.BackPageCharacter;
        }

        private List<string> GenerateAllowedInput(int count, bool canNext = false, bool canPrev = false,
            string extra = "") {
            var output = new StringBuilder();
            if (count == 0) output.Append('*');
            for (var i = 1; i <= count; i++) {
                output.Append(i);
                if (i != count) output.Append(',');
            }

            if (canNext) output.Append($",{_appSettings.NextPageCharacter}");
            if (canPrev) output.Append($",{_appSettings.BackPageCharacter}");
            return $"{output},{extra}".Split(',')
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();
        }
    }
}