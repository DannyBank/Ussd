using System.Collections.Generic;

namespace Dbank.Digisoft.Ussd.SDK.Session.Models
{
    public class MenuCollection
    {
        private bool? _input;
        public MenuCollection():this(""){ }

        public MenuCollection(string header)
        {
            Header = header;
        }
        public MenuCollection( List<UssdMenuItem> menuItems):this("")
        {
            MenuItems = menuItems;
        }

        public MenuCollection( List<UssdMenuItem> menuItems, string header):this(menuItems)
        {
            Header = header;
        }

        public  List<UssdMenuItem> MenuItems { get; set; }  = new List<UssdMenuItem>();
        public string Header { get; set; }

        public bool RequiresInput
        {
            get => _input is null ? Count > 0 : _input.Value;
            
            set => _input = value;
        }

        public int CurrentPage { get; set; } = 1;
        public int Count => MenuItems.Count;
        public bool CanBackTo { get; set; } = true;
        public bool CanBackFrom { get; set; } = true;
    }
}