﻿namespace SCD.Services.CartAPI.Models.Dto
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; } = "";
        public double CartTotal { get; set; }
    }
}
