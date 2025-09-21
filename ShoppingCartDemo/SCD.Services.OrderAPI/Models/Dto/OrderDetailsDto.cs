using System.ComponentModel.DataAnnotations.Schema;

namespace SCD.Services.OrderAPI.Models.Dto
{
    public class OrderDetailsDto
    {
        public int OrderDetailId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductCount { get; set; }
        public ProductDto Product { get; set; }
    }
}
