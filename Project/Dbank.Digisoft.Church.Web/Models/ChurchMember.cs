namespace Dbank.Digisoft.Church.Web.Models
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

    public class ChurchMember
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public HashSet<Church> Churches { get; set; }
    }
}
