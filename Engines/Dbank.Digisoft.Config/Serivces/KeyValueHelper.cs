using Dbank.Digisoft.Config.Abstractions;

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

        public async Task<Dictionary<string, string>?> GetContent(string key)
        {
            try
            {
                var path = Path.Combine(_kvUrl, key);
                var contents = _fileHelper.GetContents(path);
                return await Task.FromResult(contents.Contents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred processing {Method}", nameof(GetContent));
                return null;
            }
        }
    }
}
