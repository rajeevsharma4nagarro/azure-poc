using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SCD.Services.OrderAPI.Models.Dto;

namespace SCD.Services.OrderAPI.Service.IService
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCart(string userId);
        Task<ResponseDto> RemoveCart(string userId);
    }
}
