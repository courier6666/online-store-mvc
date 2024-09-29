﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.DataTransferObjects
{
    public class ProductDetailsDto
    {
        public Guid? Id { get; set; }
        public ProductDto Product { get; set; }
        public uint ItemsCount { get; set; }
        public Guid? OrderId { get; set; }
    }
}