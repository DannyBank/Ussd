using Dbank.Digisoft.Church.Ussd.Menus;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Church.Ussd.MenuViews {
    public abstract class MenuView<T> : IMenuHandler where T : BaseMenu
    {
        private readonly bool _override;
        private readonly string _genericErrorMsg = "An error occurred while processing your request. Please try again.";
        private readonly Dictionary<string, Func<UssdMenuItem, SessionInfo, Task<MenuCollection>>> _menuHandlerFuncs = new ();

        public MenuView(bool overide = true)
        {
            _override = overide;
            _menuHandlerFuncs.TryAdd(String.Empty, async (item, session) =>
                await Task.FromResult(new MenuCollection(_genericErrorMsg)));
            InitMenuItemHandlerFuncs();
        }

        public Type MenuType
        {
            get => typeof(T);
        }

        public bool Override
        {
            get => _override;
        }

        public virtual async Task<MenuCollection> HandlerFunc(UssdMenuItem item, SessionInfo sessionData = null)
        {
            var data = item.Data as T;
            var menuCollection = await _menuHandlerFuncs[data?.Handler ?? String.Empty].Invoke(item, sessionData);
            return menuCollection;
        }

        private void InitMenuItemHandlerFuncs()
        {
            var methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in methods)
            {
                if (HandlerOperator.MethodHasAttribute(method))
                {
                    var handler = HandlerOperator.GetHandler(this, method);
                    var handlerName = HandlerOperator.GetHandlerName(method);
                    _menuHandlerFuncs.TryAdd(handlerName, handler);
                }
            }
        }

    }
}
