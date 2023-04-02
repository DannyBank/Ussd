namespace Dbank.Digisoft.PrediBet.Api.Data
{
    public class AppSettings
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public double WaitSeconds { get; set; }
        public int SmsBatchCount { get; set; }
    }
}
