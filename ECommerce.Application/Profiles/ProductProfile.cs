using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ECommerce.Application.DTO_s.Products;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductsBrand, BrandDto>();
            CreateMap<ProductsType, TypeDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                .ReverseMap();
               
        }
    }
}
