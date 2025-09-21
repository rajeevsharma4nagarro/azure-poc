using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCD.Services.ProductAPI.Data;
using SCD.Services.ProductAPI.Models;
using SCD.Services.ProductAPI.Models.Dto;
using SCD.Services.ProductAPI.Utility;
using SCD.Services.ProductAPI.Utility.IUtility;
using System.Threading.Tasks;

namespace SCD.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _responseDto;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUploadFilesToBlob _uploadFilesToBlob;
        private readonly IConfiguration _configuration;
        public ProductAPIController(AppDbContext db, IMapper mapper1, IHttpContextAccessor httpContextAccessor,
            IUploadFilesToBlob uploadFilesToBlob, IConfiguration configuration)
        {
            _db = db;
            _responseDto = new ResponseDto();
            _mapper = mapper1;
            _httpContextAccessor = httpContextAccessor;
            _uploadFilesToBlob = uploadFilesToBlob;
            _configuration = configuration;
        }

        private string? ImageBaseUrl()
        {
            //var request = _httpContextAccessor.HttpContext?.Request;
            //if (request == null) return string.Empty;
            //return $"{request.Scheme}://{request.Host}{request.PathBase}";
            return _configuration.GetSection("StorageAccount:UrlInitial").Value;
        }

        [HttpGet]
        public object Get()
        {
            try
            {
                var list = _db.Products.Select(p => new ProductDto
                {
                    Category = p.Category,
                    Description = p.Description,
                    ID = p.ID,
                    ImageUrl = ImageBaseUrl() + p.ImageUrl,
                    Name = p.Name,
                    Price = p.Price
                }).ToList();
                _responseDto.Result = list;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public object Get(int id)
        {
            try
            {
                Product obj = _db.Products.First(p => p.ID == id);
                _responseDto.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<object> Post([FromForm] ProductUploadDto request)
        {
            try
            {
                if (request.Image == null)
                {
                    throw new Exception("Invalid data");
                }

                var product = new Product();
                product.Name = request.Name;
                product.Description = request.Description;
                product.Category = request.Category;
                product.Price = request.Price;

                string fileUrl = await _uploadFilesToBlob.UploadToBlob(request.Image);

                //var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                //if (!Directory.Exists(uploadFolder))
                //{
                //    Directory.CreateDirectory(uploadFolder);
                //}
                //var filename = Guid.NewGuid().ToString() + Path.GetExtension(request.Image.FileName);
                //var filepath = Path.Combine(uploadFolder, filename);
                //using(var stream = new FileStream(filepath, FileMode.Create))
                //{
                //    await request.Image.CopyToAsync(stream);
                //}
                //product.ImageUrl = "/images/" + filename;

                product.ImageUrl = fileUrl;

                _db.Products.Add(product);
                _db.SaveChanges();
                _responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}
