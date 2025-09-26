using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCD.MessageBus;
using SCD.Services.CartAPI.Data;
using SCD.Services.CartAPI.Models;
using SCD.Services.CartAPI.Models.Dto;
using SCD.Services.CartAPI.Service.IService;
using System.Reflection.PortableExecutable;

namespace SCD.Services.CartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _responseDto;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;
        private readonly iMessageBus _iMessageBus;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CartController> _logger;
        public CartController(AppDbContext db, IMapper mapper1, IHttpContextAccessor httpContextAccessor
            , IProductService productService, iMessageBus messageBus, IConfiguration configuration, ILogger<CartController> logger)
        {
            _db = db;
            _responseDto = new ResponseDto();
            _mapper = mapper1;
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
            _iMessageBus = messageBus;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                if (string.IsNullOrEmpty(cartDto.CartHeader.UserId))
                {
                    throw new Exception("Envalid api call");
                }
                var carHeaderDb = await _db.cartHeaders.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == cartDto.CartHeader.UserId);

                if (carHeaderDb == null)
                {
                    //create header & details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.cartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.cartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                 }
                else
                {
                    var cartDetailsDb = await _db.cartDetails.AsNoTracking().FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetails.First().ProductId && 
                    u.CartHeaderId == carHeaderDb.CartHeaderId);
                    if(cartDto.CartDetails.First().Price == 0)
                    {
                        int total = _db.cartDetails.Where(d => d.CartHeaderId == cartDetailsDb.CartHeaderId).Count();
                        _db.cartDetails.Remove(cartDetailsDb);
                        if (total == 1)
                        {
                            var cartHeaderToRemove = await _db.cartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetailsDb.CartHeaderId);
                            _db.cartHeaders.Remove(cartHeaderToRemove);
                        }
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        if (cartDetailsDb == null)
                        {
                            //add item
                            cartDto.CartDetails.First().CartHeaderId = carHeaderDb.CartHeaderId;
                            _db.cartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                            await _db.SaveChangesAsync();
                        }
                        else
                        {
                            //update count
                            if(cartDetailsDb.Count == 1)
                                cartDto.CartDetails.First().Count += cartDetailsDb.Count;
                            else
                                cartDto.CartDetails.First().Count = cartDetailsDb.Count;

                            cartDto.CartDetails.First().CartHeaderId = cartDetailsDb.CartHeaderId;
                            cartDto.CartDetails.First().CartDetailId = cartDetailsDb.CartDetailId;
                            _db.cartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                            await _db.SaveChangesAsync();
                        }
                    }
                }
                _responseDto.Result = cartDto;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }

            return _responseDto;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.cartDetails.First(u => u.CartDetailId == cartDetailsId);
                int total = _db.cartDetails.Where(d => d.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.cartDetails.Remove(cartDetails);
                if (total == 1)
                {
                    var cartHeaderToRemove = await _db.cartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    _db.cartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();
                _responseDto.Result = true;

            }
            catch (Exception ex)
            {
                _logger.LogError("Cart api remove cart Failed: " + ex.Message);
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }

            return _responseDto;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartResponseDto cart = new CartResponseDto()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.cartHeaders.First(u => u.UserId == userId)),
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsResponseDto>>(
                    _db.cartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId
                    ));

                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(u => u.ID == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Price);
                }

                _responseDto.Result = cart;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpPost("ClearCart")]
        public async Task<ResponseDto> ClearCart([FromBody] string userId)
        {
            try
            {
                var lst = _db.cartHeaders.Where(x => x.UserId == userId).ToList();
                _db.cartHeaders.RemoveRange(lst);
                await _db.SaveChangesAsync();

                _responseDto.Result = "Cart has been cleared.";
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }
    }
}
