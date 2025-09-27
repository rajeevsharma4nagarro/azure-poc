using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCD.Services.OrderAPI.Models.Dto;
using SCD.Services.OrderAPI.Service.IService;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SCD.Services.OrderAPI.Service
{
    public class CartService : ICartService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ResponseDto responseDto;
        public CartService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            responseDto = new ResponseDto();
        }

        public async Task<CartResponseDto> GetCart(string userId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CartApi");
                var tokent = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(tokent))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokent.Replace("Bearer ", ""));
                }
                var response = await client.GetAsync($"/api/cart/GetCart/{userId}");
                var apiresponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResponseDto>(apiresponse);
                if (result.IsSuccess)
                {
                    return JsonConvert.DeserializeObject<CartResponseDto>(Convert.ToString(result.Result));
                }
                else
                {
                    Console.WriteLine(String.Concat("GetCart Failed: client:${0} and  tokent:${1}", client, tokent));
                    throw new Exception(String.Concat("GetCart Failed: client:${0} and  tokent:${1}", client, tokent));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Inside CreateOrder:" + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public async Task<ResponseDto> RemoveCart(string userId)
        {
            var client = _httpClientFactory.CreateClient("CartApi");
            var tokent = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(tokent))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokent.Replace("Bearer ", ""));
            }

            var response = await client.PostAsJsonAsync($"/api/cart/ClearCart", userId);
            if (response.IsSuccessStatusCode)
            {
                var apiresponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResponseDto>(apiresponse);
                if (result.IsSuccess)
                {
                    return result;
                } else
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "Operation failed";
                }
            } 
            else
            {
                responseDto.IsSuccess = false;
                responseDto.Message = response.ReasonPhrase;
            }
            return responseDto;
        }
    }
}
