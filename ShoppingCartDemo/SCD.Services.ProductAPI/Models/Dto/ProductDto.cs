using System.ComponentModel.DataAnnotations;

namespace SCD.Services.ProductAPI.Models.Dto
{
    public class ProductDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double Price { get; set; } = double.MaxValue;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
