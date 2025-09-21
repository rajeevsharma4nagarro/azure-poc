using SCD.Services.OrderAPI.Models.Dto;
using SCD.Services.OrderAPI.Service.IService;
using System.Text;
using System.Text.Json;

namespace SCD.Services.OrderAPI.Service
{
    public class EmailService: IEmailService
    {
        IHttpClientFactory _httpClient;
        ResponseDto _responseDto;
        public EmailService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory;
            _responseDto = new ResponseDto();
        }

        public async Task<ResponseDto> SendEmailAsync(string to, string subject, string mailbody)
        {

            var payload = new
            {
                to = to,
                subject = subject,
                mailbody = mailbody
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var jsonContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");


            var client = _httpClient.CreateClient("EmailTriggerLogicApp");
            HttpResponseMessage response = await client.PostAsync(client.BaseAddress, jsonContent);
            if (response.IsSuccessStatusCode)
            {
                _responseDto.Result = response;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                _responseDto.IsSuccess = false;
                _responseDto.Message = error;
            }
            return _responseDto;
        }
    }
}
