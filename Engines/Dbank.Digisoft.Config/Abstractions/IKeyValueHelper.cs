namespace Dbank.Digisoft.Config.Abstractions
{
    public interface IKeyValueHelper
    {
        Task<Dictionary<string, string>?> GetContent(string key);
    }
}
