using SCD.EmailProcessorFunction.Models;
using System.Text;
using System.Text.Json;

namespace SCD.EmailProcessorFunction.Services
{
    public class EmailService
    {
        IHttpClientFactory _httpClient;
        ResponseDto _responseDto;
        public EmailService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory;
            _responseDto = new ResponseDto();
        }

        public async Task<ResponseDto> SendEmailAsync(OrderHeader orderHeader)
        {
            string to = orderHeader.Email;
            string subject = @"SCD - Order status update.";
            string mailbody = $@"Hi {orderHeader.Name},<br/><br/>
Your order no {orderHeader.OrderHeaderId} for amount of Rs.{orderHeader.OrderTotal}/- has been <b>{orderHeader.Status}</b>.
<br/>You can check order status on SCD portal. 
<br/><br/><br/>--<br/>Thanks<br/>Shopping Cart Demo (SCD)<br/>";

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

