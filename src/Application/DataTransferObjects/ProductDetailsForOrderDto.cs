using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.DataTransferObjects
{
    public class ProductDetailsForOrderDto
    {
        public uint ItemsCount { get; set; }
        public ProductDto Product { get; set; }
    }
}
