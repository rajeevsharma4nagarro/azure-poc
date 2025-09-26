using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SCD.MessageBus;
using SCD.Services.OrderAPI.Data;
using SCD.Services.OrderAPI.Models;
using SCD.Services.OrderAPI.Models.Dto;
using SCD.Services.OrderAPI.Service;
using SCD.Services.OrderAPI.Service.IService;
using SCD.Services.OrderAPI.Utilitiy;

namespace SCD.Services.OrderAPI.Controllers
{
    [Route("api/checkout")]
    [ApiController]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _responseDto;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly iMessageBus _iMessageBus;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public CheckoutController(AppDbContext db, IMapper mapper1, IHttpContextAccessor httpContextAccessor
            , IProductService productService, ICartService cartService, iMessageBus messageBus, IConfiguration configuration,
            IEmailService emailService)
        {
            _db = db;
            _responseDto = new ResponseDto();
            _mapper = mapper1;
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
            _cartService = cartService;
            _iMessageBus = messageBus;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartCheckoutDto cartCheckoutDto)
        {
            try
            {
                var CartResponseDto = await _cartService.GetCart(cartCheckoutDto.UserId);

                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                var orderDetails = new List<OrderDetails>();
                foreach (var details in CartResponseDto.CartDetails)
                {
                    orderDetails.Add(new OrderDetails
                    {
                        ProductCount = details.Count,
                        ProductId = details.ProductId,
                        ProductName = details.Product.Name,
                        ProductPrice = details.Price,
                        TotalPrice = (details.Count * details.Price)
                        //OrderHeaderId = details.CartHeaderId,
                        //OrderDetailId = ,
                        //OrderHeader = ,
                        //Product = 
                    });
                }

                var orderHeader = new OrderHeader()
                {
                    UserId = cartCheckoutDto.UserId,
                    Email = cartCheckoutDto.Email,
                    Name = cartCheckoutDto.Name,
                    Address = cartCheckoutDto.Address,
                    City = cartCheckoutDto.City,
                    State = cartCheckoutDto.State,
                    Zip = cartCheckoutDto.Zip,
                    Payment = cartCheckoutDto.Payment,
                    CardNumber = cartCheckoutDto.CardNumber,
                    Expiry = cartCheckoutDto.Expiry,
                    Cvv = cartCheckoutDto.Cvv,

                    OrderOn = DateTime.Now,
                    OrderTotal = orderDetails.Sum(x => x.TotalPrice),

                    OrderDetails = orderDetails,
                    //OrderHeaderId   = 1,
                    Status = OrderStatus.Pending
                };

                OrderHeader ordercreated = _db.OrderHeaders.Add(orderHeader).Entity;
                await _db.SaveChangesAsync();

                //Clear Cart Items
                var clearresponse = await _cartService.RemoveCart(cartCheckoutDto.UserId);
                if (!clearresponse.IsSuccess)
                {
                    throw new Exception("Cart Remove  Failed:" + clearresponse.Message);
                }

                OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(ordercreated);
                orderHeaderDto.OrderHeaderId = ordercreated.OrderHeaderId;

                orderHeaderDto.OrderDetails = [];
                _responseDto.Result = orderHeaderDto;

                try
                {
                    string to = orderHeaderDto.Email;
                    string subject = "SCD - Order confirmation";
                    string mailbody = $@"Hi {orderHeaderDto.Name},<br/><br/>
Your order has been received with order no {orderHeaderDto.OrderHeaderId} for the amount of Rs. {orderHeaderDto.OrderTotal}/-.
<br/>You can check order status on SCD portal. 
<br/><br/><br/>--<br/>Thanks<br/>Shopping Cart Demo (SCD)<br/>";
                    var resp = await _emailService.SendEmailAsync(to, subject, mailbody);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to send email: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpGet("GetOrders/{userId}")]
        public async Task<ResponseDto> GetOrders(string UserId)
        {
            try
            {
                IEnumerable<OrderHeader> orderHeader = _db.OrderHeaders.Where(x => x.UserId == UserId);

                _responseDto.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(orderHeader);
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpGet("GetPendingOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> GetPendingOrders()
        {
            try
            {
                IEnumerable<OrderHeader> orderHeader = _db.OrderHeaders.Where(x => x.Status == OrderStatus.Pending);
                _responseDto.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(orderHeader);
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        [HttpPost("UpdateOrderStatus")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> UpdateOrderStatus([FromBody] UpdateStatusDto updateStatusDto)
        {
            try
            {
                OrderHeader orderHeader = _db.OrderHeaders.Where(x => x.OrderHeaderId == updateStatusDto.OrderHeaderId).First();
                if (orderHeader != null)
                {
                    orderHeader.Status = (updateStatusDto.IsApproved == true) ? OrderStatus.Approved : OrderStatus.Reject;
                    _db.OrderHeaders.Update(orderHeader);
                    _db.SaveChanges();

                    _responseDto.Result = orderHeader;

                    // Push message in message queue
                    await _iMessageBus.PublishMessage(orderHeader);
                }
                else
                {
                    _responseDto.Message = "Record not found";
                    _responseDto.IsSuccess = false;
                }
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
