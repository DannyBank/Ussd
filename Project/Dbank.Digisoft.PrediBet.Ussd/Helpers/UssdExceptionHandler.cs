﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Abstractions;

namespace Dbank.Digisoft.PrediBet.Ussd.Helpers
{
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