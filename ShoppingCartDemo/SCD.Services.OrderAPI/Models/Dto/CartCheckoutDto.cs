namespace SCD.Services.OrderAPI.Models.Dto
{
    public class CartCheckoutDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Payment { get; set; }
        public string CardNumber { get; set; }
        public string Expiry { get; set; }
        public string Cvv { get; set; }
    }
}
