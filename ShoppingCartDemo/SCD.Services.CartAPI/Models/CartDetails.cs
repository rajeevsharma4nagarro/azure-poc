using SCD.Services.CartAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCD.Services.CartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        [NotMapped]
        public ProductDto Products { get; set; }
        public int Count { get; set; }
    }
}
