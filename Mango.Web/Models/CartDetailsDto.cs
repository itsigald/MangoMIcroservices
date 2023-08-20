﻿namespace Mango.Web.Models
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }

        public int CartHeaderId { get; set; }

        //public CartHeaderDto? CartHeader { get; set; } = null;

        public int ProductId { get; set; }

        public ProductDto? Product { get; set; }

        public int Quantity { get; set; }
    }
}
