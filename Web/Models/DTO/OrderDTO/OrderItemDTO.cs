﻿namespace Web.Models.DTO.OrderDTO
{
    public class OrderItemDTO
    {
        public int Amount { get; set; }
        public double Price { get; set; }
        public ProductDTO Product { get; set; }
    }
}
