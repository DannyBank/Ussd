using Dbank.Digisoft.Ussd.SDK.Models;
using Dbank.Digisoft.Ussd.SDK.Session.Abstractions;
using Dbank.Digisoft.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Church.Ussd.Helpers {
    public class UssdExceptionHandler
    {
        private readonly ILogger<UssdExceptionHandler> _logger;
        private readonly AppSettings _appSettings;
        private readonly AppStrings _appStrings;
        private readonly ISessionServiceHelper _sessionHelper;

        public UssdExceptionHandler(
            ILogger<UssdExceptionHandler> logger,
            IOptionsSnapshot<AppSettings> appSettings,
            IOptionsSnapshot<AppStrings> appStrings,
            ISessionServiceHelper sessionHelper)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _appStrings = appStrings.Value;
            _sessionHelper = sessionHelper;

        }

        public async Task<UssdMenu> ExceptionHandler(string MethodName, SessionInfo session, Exception e)
        {
            _logger.LogError(e, "Error Occured Executing {MethodName} for {Msisdn}", MethodName, session?.Msisdn ?? "");

           
            return new UssdMenu
            {
                Header = _appStrings.GenericError,
                ExpectInput = false
            };
        }
    }
}