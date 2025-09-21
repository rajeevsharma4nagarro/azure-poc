using SCD.Services.OrderAPI.Models.Dto;

namespace SCD.Services.OrderAPI.Service.IService
{
    public interface IEmailService
    {
        Task<ResponseDto> SendEmailAsync(string to, string subject, string mailbody);
    }
}
