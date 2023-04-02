using Dbank.Digisoft.PrediBet.Ussd.Data.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController
    {
        public readonly IApplicationDataHelper _appHelper;

        public NotificationController(IApplicationDataHelper appHelper)
        {
            _appHelper = appHelper;
        }

        [HttpPost("[action]")]
        public async Task<SmsOutbox> QueueSms(SmsOutbox request)
        {
            return await _appHelper.SaveSmsOutbox(request);
        }
    }
}
