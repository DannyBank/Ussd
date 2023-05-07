using Newtonsoft.Json.Linq;

namespace Dbank.Digisoft.Config.Abstractions
{
    public interface IKeyValueHelper
    {
        Task<JObject?> GetJsonContent(string key);
    }
}
