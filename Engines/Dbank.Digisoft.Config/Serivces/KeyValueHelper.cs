using Dbank.Digisoft.Config.Abstractions;
using Newtonsoft.Json.Linq;

namespace Dbank.Digisoft.Config.Serivces
{
    public class KeyValueHelper: IKeyValueHelper
    {
        private readonly ILogger<KeyValueHelper> _logger;
        private readonly IFileHelper _fileHelper;
        private readonly string _kvUrl;

        public KeyValueHelper(ILogger<KeyValueHelper> logger, IFileHelper fileHelper)
        {
            _logger = logger;
            _fileHelper = fileHelper;
            _kvUrl = Path.Combine(Environment.CurrentDirectory, "bin", "Debug", "net7.0", "KeyValues");
        }

        public async Task<JObject?> GetJsonContent(string key)
        {
            try
            {
                var environment = Path.Combine(_kvUrl, key);
                var paths = _fileHelper.GetDirectoriesAndFiles(environment);
                var jObjects = await _fileHelper.GetContents(paths!.DirectoriesAndFiles);
                return MergeJsonObjects(jObjects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred processing {Method}", nameof(GetJsonContent));
                return null;
            }
        }

        private static JObject MergeJsonObjects(List<JObject> objects)
        {
            JObject json = new ();
            foreach (JObject JSONObject in objects)
            {
                foreach (var property in JSONObject)
                {
                    string name = property.Key;
                    JToken value = property.Value;
                    json.Add(name, value);
                }
            }
            return json;
        }
    }
}
