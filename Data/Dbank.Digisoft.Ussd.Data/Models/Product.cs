namespace Dbank.Digisoft.Ussd.Data.Models
{
    public class Product
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int VendorId { get; set; }
        public string Uom { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductCode { get; set; }
        public int DiscountAmount { get; set; }

    }
}
