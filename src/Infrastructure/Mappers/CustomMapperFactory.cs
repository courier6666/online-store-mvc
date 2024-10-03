using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Store.Application.DataTransferObjects;
using Store.Application.DataTransferObjects;
using Store.Application.Interfaces.Mapper;
using Store.Application.DataTransferObjects;
using Store.Domain.Entities;
using Store.Domain.Entities.Model;
using Store.Domain.Entities.Interfaces;

namespace Store.Infrastructure.Mappers
{
    /// <summary>
    /// Factory pattern, used to create AutoMapperAdapters
    /// </summary>
    public static class CustomMapperFactory
    {
        /// <summary>
        /// Creates <typeparamref name="IMapper"/> from AutoMapper library with all configurations for mapping.
        /// Wraps <typeparamref name="IMapper"/> with <typeparamref name="AutoMapperAdapter"/>.
        /// Configurations include mapping from Domain Entity Models to Data-Transfer objects and vice-versa.
        /// </summary>
        /// <returns>Instance of <typeparamref name="AutoMapperAdapter"/></returns>
        public static AutoMapperAdapter Create()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CashDeposit, CashDepositDto>();
                cfg.CreateMap<Address, AddressDto>();
                cfg.CreateMap<AddressDto, Address>();
                cfg.CreateMap<UserRegistrationDto, IUser>().ForMember(u => u.Address, 
                    o => o.MapFrom(udto => udto.Address)).
                    ForMember(u => u.Birthday, o => o.MapFrom(udto => udto.Birthday)).
                    ForMember(udto => udto.PasswordHash, opt => opt.Ignore());
                cfg.CreateMap<ProductDto, Product>();
                cfg.CreateMap<Product, ProductDto>().
                    ForMember(p => p.DateOfCreation, o => o.MapFrom(p => p.CreatedDate));
                cfg.CreateMap<ProductDetailsDto, OrderProductDetail>();
                cfg.CreateMap<OrderProductDetail, ProductDetailsForOrderDto>();
                cfg.CreateMap<Entry, EntryDto>();
                cfg.CreateMap<Order, OrderDto>().ForMember(o => o.TotalPrice,
                        o => o.MapFrom(or => or.TotalPrice))
                    .ForMember(o => o.ProductDetails, o => o.MapFrom(o => o.ProductDetails))
                    .ForMember(o => o.Status, o => o.MapFrom(o => o.Status.ToString()));
            });
            
            return new AutoMapperAdapter(mapperConfig.CreateMapper());
        }
    }
}
