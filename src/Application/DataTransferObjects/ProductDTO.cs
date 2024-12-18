﻿namespace Store.Application.DataTransferObjects
{
    public class ProductDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsRemovedFromPageStore { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
