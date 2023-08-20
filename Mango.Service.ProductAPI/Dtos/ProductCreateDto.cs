﻿namespace Mango.Services.ProductAPI.Dtos
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}
