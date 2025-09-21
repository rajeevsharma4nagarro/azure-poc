using SCD.Services.CartAPI.Models.Dto;

namespace SCD.Services.CartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
