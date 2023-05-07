using Dbank.Digisoft.Config.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dbank.Digisoft.Config.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KVController : ControllerBase
    {
        private readonly ILogger<KVController> _logger;
        private readonly IKeyValueHelper _kvHelper;

        public KVController(ILogger<KVController> logger, IKeyValueHelper kvHelper)
        {
            _logger = logger;
            _kvHelper = kvHelper;
        }

        [HttpGet("get/{app}/{env}")]
        public async Task<string> Get(string app, string env)
        {
            try
            {
                if (!ModelState.IsValid) return null!;
                var result = await _kvHelper.GetJsonContent(Path.Combine(app,env));
                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in endpoint {Method}", nameof(Get));
                return null!;
            }
        }
    }
}
