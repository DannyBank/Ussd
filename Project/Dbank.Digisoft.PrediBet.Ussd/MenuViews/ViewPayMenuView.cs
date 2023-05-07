using Dbank.Digisoft.PrediBet.Ussd;
using Dbank.Digisoft.PrediBet.Ussd.MenuViews;
using Dbank.Digisoft.Ussd.Data.Abstractions;
using Dbank.Digisoft.Ussd.Data.Models;
using Dbank.Digisoft.Ussd.Data.Models.PrediBet;
using Dbank.Digisoft.Ussd.Menus;
using Dbank.Digisoft.Ussd.SDK.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Models;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartFormat;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dbank.Digisoft.MenuViews
{
    public class ViewPayMenuView : MenuView<ViewPayMenu> {
        private readonly ILogger<ViewPayMenuView> _logger;
        private readonly MenuData _menuData;
        private readonly AppSettings _appSettings;
        private readonly AppStrings _appStrings;
        private readonly IPrediBetClient _appHelper;

        public ViewPayMenuView(ILogger<ViewPayMenuView> logger,
            IOptionsSnapshot<MenuData> menuData,
            IOptionsSnapshot<AppSettings> appSettings,
            IOptionsSnapshot<AppStrings> appStrings,
            IPrediBetClient db) {
            _logger = logger;
            _menuData = menuData.Value;
            _appSettings = appSettings.Value;
            _appStrings = appStrings.Value;
            _appHelper = db;
        }

        [Handler("Index")]
        private async Task<MenuCollection> Index(UssdMenuItem input, SessionInfo sessionData = null) {
            //Save selected Booking set
            var menuData = input.Data as ViewPayMenu;
            if (menuData == null)
                return new MenuCollection(_appStrings.InvalidBookingCode);
            sessionData.StoreSessionData(AppConstants.SELECTED_BOOKING_CODE, menuData.Text);
            var bookingSet = await _appHelper.GetBookingsByCode(menuData.Text);
            sessionData.StoreSessionData(AppConstants.SELECTED_BOOKING_SET, bookingSet);

            //Display View/Pay menu
            var menuItems = new List<UssdMenuItem>();
            menuItems.AddRange(_menuData.ViewPayMenu.Select(r => new UssdMenuItem() {
                Data = new ViewPayMenu {
                    Text = r.Text,
                    Handler = r.Handler,
                    BookingCode = menuData.Text
                },
                DataType = typeof(ViewPayMenu),
                Text = r.Text,
                Position = r.Position
            }));
            return await Task.FromResult(new MenuCollection(menuItems, _appStrings.SelectToViewOrPay));
        }

        [Handler("DisplayBooking")]
        private async Task<MenuCollection> DisplayBooking(UssdMenuItem input, SessionInfo sessionData = null) {
            var menuData = input.Data as ViewPayMenu;
            if (menuData == null) return new(_appStrings.InvalidBookingSelected);

            var bookingCode = menuData.BookingCode 
                ?? sessionData.GetSessionData<string>(AppConstants.SELECTED_BOOKING_CODE);
            var bookings = sessionData.GetSessionData<List<Booking>>(AppConstants.SELECTED_BOOKING_SET)
                ?? await _appHelper.GetBookingsByCode(bookingCode);
            if (bookings == null) return new(_appStrings.InvalidBookingCode);
            if (bookings.Count == 0) return new(_appStrings.NoBookingsForCode);

            var menuItems = new List<UssdMenuItem>();
            menuItems.AddRange(bookings.Select(r => new UssdMenuItem (){
                Data = new ViewPayMenu {
                    Text = r.Code,
                    Handler = nameof(ChangeBooking),
                    BookingId = r.BookingId,
                    Prediction = menuData.Prediction
                },
                DataType = typeof(ViewPayMenu),
                Text = r.ToString()
            }));
            var isBookingUpdated = sessionData.GetSessionData<bool>(AppConstants.BOOKING_SET_UPDATED);
            if (isBookingUpdated)
                menuItems.Add(new UssdMenuItem() {
                    Data = new ViewPayMenu {
                        Text = "Pay",
                        Handler = "PaymentPrompt"
                    },
                    DataType = typeof(ViewPayMenu),
                    Text = "Pay"
                });
            return new(menuItems, _appStrings.SelectToUpdate);
        }

        [Handler("ChangeBooking")]
        private async Task<MenuCollection> ChangeBooking(UssdMenuItem input, SessionInfo sessionData = null) {
            var menuData = input.Data as ViewPayMenu;
            if (menuData == null) return new(_appStrings.InvalidBookingSelected);

            var menuItems = new List<UssdMenuItem>();
            menuItems.AddRange(_menuData.PredictionMenu
                    .Select(r => new UssdMenuItem {
                        Text = r.Text,
                        Data = new ViewPayMenu {
                            Handler = r.Handler,
                            Text = r.Text,
                            NextMenuType = r.NextMenuType,
                            BookingId = menuData.BookingId,
                            Prediction = r.PredictionValue
                        },
                        DataType = typeof(ViewPayMenu),
                        Position = r.Position
                    }));

            sessionData.StoreSessionData(AppConstants.SELECTED_BOOKING_ID, menuData.BookingId);
            var header = Smart.Format(_appStrings.ChangeBookingPrompt, new { match = input.Text });
            return await Task.FromResult(new MenuCollection(menuItems, header));
        }

        [Handler("PaymentPrompt")]
        private async Task<MenuCollection> PaymentPrompt(UssdMenuItem input, SessionInfo sessionData = null) {
            StoreNextMenu(sessionData, nameof(ProcessPayment), "ViewPayMenu");
            return await Task.FromResult(new MenuCollection(_appStrings.EnterAmount) {
                RequiresInput = true
            });
        }

        [Handler("ProcessPayment")]
        private async Task<MenuCollection> ProcessPayment(UssdMenuItem input, SessionInfo sessionData = null) {
            var selectedBooking = sessionData.GetSessionData<List<Booking>>(AppConstants.SELECTED_BOOKING_SET);
            var result = await ProcessMoMoPayment(selectedBooking, input, sessionData);
            return
                new(result.Success ? _appStrings.ProcessPaymentPrompt: _appStrings.GenericError)
                { RequiresInput = false };
        }

        [Handler("SelectPrediction")]
        private async Task<MenuCollection> SelectPrediction(UssdMenuItem input, SessionInfo sessionData = null) {
            var menuData = input.Data as ViewPayMenu;
            if (menuData == null) return new(_appStrings.InvalidPredictionSelected);

            await UpdateBookings(menuData, sessionData);
            return await DisplayBooking(input, sessionData);
        }

        private async Task UpdateBookings(ViewPayMenu menuData, SessionInfo sessionData) {
            //get booking Id from either previous menu data or session
            var bookingId = menuData.BookingId > 0 ?
                menuData.BookingId : sessionData.GetSessionData<long>(AppConstants.SELECTED_BOOKING_ID);
            var bookingCode = sessionData.GetSessionData<string>(AppConstants.SELECTED_BOOKING_CODE);

            var bookingSet = sessionData.GetSessionData<List<Booking>>(AppConstants.SELECTED_BOOKING_SET)
                ?? await _appHelper.GetBookingsByCode(bookingCode);

            //replace current predicition with new one
            bookingSet.Find(r => r.BookingId == bookingId).Prediction = menuData.Prediction;
            sessionData.StoreSessionData(AppConstants.SELECTED_BOOKING_SET, bookingSet);
            sessionData.StoreSessionData(AppConstants.BOOKING_SET_UPDATED, true);
        }

        private static void StoreNextMenu(SessionInfo session, string handler, string nextMenuType) {
            var nextMenuItem = new List<UssdMenuItem>
            {
                new UssdMenuItem
                {
                    DataType = typeof(ViewPayMenu),
                    Data = new ViewPayMenu
                    {
                        Handler = handler,
                        NextMenuType = nextMenuType
                    }
                }
            };
            var nextMenu = new MenuCollection(nextMenuItem);
            session.StoreSessionData(AppConstants.MENUITEMS, nextMenu);
        }

        private async Task<MomoPaymentResult> ProcessMoMoPayment(List<Booking> selectedBooking,
            UssdMenuItem input, SessionInfo sessionData = null) {
            return new() { Success = true};
        }

        private async Task<List<Booking>> GetSelectedBooking(string selectedCode, SessionInfo sessionData) {

            return await Task.FromResult(new List<Booking>());
        }
    }
}
