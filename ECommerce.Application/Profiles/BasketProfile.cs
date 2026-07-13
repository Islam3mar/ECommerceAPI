using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ECommerce.Application.DTO_s.Basket;
using ECommerce.Domain.Entities.Baskets;

namespace ECommerce.Application.Profiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket,BasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();

        }
    }
}
