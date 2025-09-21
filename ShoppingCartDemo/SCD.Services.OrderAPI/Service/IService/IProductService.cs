using SCD.Services.OrderAPI.Models.Dto;

namespace SCD.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
