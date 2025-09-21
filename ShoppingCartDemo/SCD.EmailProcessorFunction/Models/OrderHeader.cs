
namespace SCD.EmailProcessorFunction.Models
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }
        public double OrderTotal { get; set; }
        public string UserId { get; set; } = "";
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Payment { get; set; }
        public string CardNumber { get; set; }
        public string Expiry { get; set; }
        public string Cvv { get; set; }
        public string Status { get; set; }
        public DateTime OrderOn { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
