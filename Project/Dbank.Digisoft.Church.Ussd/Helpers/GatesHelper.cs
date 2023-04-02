using Dbank.Digisoft.PrediBet.Ussd.Data.Extensions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Models;
using Microsoft.Extensions.Options;

namespace Dbank.Digisoft.Church.Ussd.Helpers {
    public class GatesHelper
    {
        private readonly AppSettings _appSettings;
        private readonly AppStrings _appStrings;


        public GatesHelper(
            IOptionsSnapshot<AppSettings> appSettings,
            IOptionsSnapshot<AppStrings> appStrings)
        {
            _appSettings = appSettings.Value;
            _appStrings = appStrings.Value;
           
        }

        public async Task<(bool,string)> CheckAllowed(UserInput data)
        {
            try
            {
                //Check TestMode 
                if (!IsTestNumber(data.Msisdn))
                    return (false, _appStrings.NotAllowed);

                //Check Barred Numbers
                if (IsBarredNumber(data.Msisdn))
                    return (false, _appStrings.BarredMsisdn);

                //Check ShortCode
                if (!IsValidShortCode(data.ShortCode))
                    return (false, _appStrings.UnknownShortCode);

                return (true, "");
            }
            catch (Exception e)
            {               
                return (false, _appStrings.GenericError);
            }
        }

        public bool IsTestNumber(string msisdn)
        {
            if (!_appSettings.IsTesting) return true;
            if (!_appSettings.Testers.Any()) return false;
            if (_appSettings.Testers.Contains(msisdn)) return true;
            return false;
        }

        public bool IsBarredNumber(string msisdn)
        {
            msisdn = msisdn.ToIntMsisdn(IntCode.GHA);
            return _appSettings.BarredNumbers?.Any(c => c == msisdn) ?? false;
        }


        public bool IsValidShortCode(string shortCode)
        {
            return _appSettings.ShortCodes.Contains(shortCode);
        }


    }
}
