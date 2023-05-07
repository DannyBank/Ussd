using Dbank.Digisoft.Ussd.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dbank.Digisoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChurchController : ControllerBase
    {
        private readonly IChurchClient _churchClient;

        public ChurchController(IChurchClient churchClient)
        {
            _churchClient = churchClient;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _churchClient.GetAllChurches());
        }
    }
}
