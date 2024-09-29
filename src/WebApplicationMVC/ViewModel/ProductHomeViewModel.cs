﻿using Store.Application.DataTransferObjects;
using Store.Application.Queries;
using Store.Domain.PagedLists;
using Store.WebApplicationMVC.Models;

namespace Store.WebApplicationMVC.ViewModel
{
    public class ProductHomeViewModel
    {
        public IEnumerable<ProductDto> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? ProductName { get; set; }
        public IEnumerable<string> AllCategories { get; set; }
        public string? CurrentValueSortedBy { get; set; }
    }
}