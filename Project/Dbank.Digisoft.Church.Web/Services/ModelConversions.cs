namespace Dbank.Digisoft.Church.Web.Services
{
    public static class ModelConversions
    {
        public static IEnumerable<Models.Church> ToChurchModel(this List<Ussd.Data.Models.ChurchModels.Church> church)
        {
            return church.Select(r => new Models.Church
            {
                ChurchId = r.ChurchId,
                Name = r.Name
            }) ?? new List<Models.Church>();
        }
    }
}
