namespace Dbank.Digisoft.Ussd.Data.Models.ChurchModels
{
    public class Church
    {
        public int ChurchId { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"ChurchId: {ChurchId} Name: {Name}";
        }
    }
}
