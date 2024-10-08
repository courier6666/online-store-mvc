﻿namespace Store.WebApplicationMVC.ViewModel
{
    public class ProductViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ReturnUrl { get; set; }
    }
}
