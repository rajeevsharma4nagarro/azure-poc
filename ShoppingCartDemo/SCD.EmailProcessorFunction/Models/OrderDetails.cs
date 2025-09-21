namespace SCD.EmailProcessorFunction.Models
{
    public class OrderDetails
    {
        public int OrderDetailId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductCount { get; set; }
        public double TotalPrice { get; set; }
    }
}
