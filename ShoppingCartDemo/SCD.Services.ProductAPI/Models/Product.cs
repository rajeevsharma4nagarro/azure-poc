using System.ComponentModel.DataAnnotations;

namespace SCD.Services.ProductAPI.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }= string.Empty;
        [Range(1,10000)]
        public string Description { get; set; }=string.Empty;
        public string Category { get; set;}= string.Empty;
        public double Price {  get; set; }= double.MaxValue;
        public string ImageUrl {  get; set; }= string.Empty;
    }
}
