using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SCD.Services.OrderAPI.Models.Dto;
using SCD.Services.OrderAPI.Service.IService;
using System.Net.Http.Headers;

namespace SCD.Services.OrderAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            //string filePath = @"mylog.txt";
            //string logTime = DateTime.Now.ToString("YYYY-MM-DD:HH-mm-ss");

            var client = _httpClientFactory.CreateClient("Product");
            //System.IO.File.AppendAllText(filePath, Environment.NewLine + Environment.NewLine + $"{logTime}  - CreateClient('Product') json: {JsonConvert.SerializeObject(client)} ");

            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            //System.IO.File.AppendAllText(filePath, Environment.NewLine + Environment.NewLine + $"{logTime}  - token : {token} ");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            }

            var response = await client.GetAsync("/api/product");
            //System.IO.File.AppendAllText(filePath, Environment.NewLine + Environment.NewLine + $"{logTime}  - GetAsync('/api/product') json: {JsonConvert.SerializeObject(response)} ");


            var apiContent = await response.Content.ReadAsStringAsync();
            //System.IO.File.AppendAllText(filePath, Environment.NewLine + Environment.NewLine + $"{logTime}  - apiContent json: {JsonConvert.SerializeObject(apiContent)} ");

            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            //System.IO.File.AppendAllText(filePath, Environment.NewLine + Environment.NewLine + $"{logTime}  - resp json: {JsonConvert.SerializeObject(resp)} ");

            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
            }
            return new List<ProductDto>();
        }
    }
}
