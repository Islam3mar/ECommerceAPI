using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ECommerce.Application.DTO_s.Identity;
using ECommerce.Application.DTO_s.Order;
using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<DeliveryMethod, DeliveryMethodDto>();

            CreateMap<OrderAddress, AddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                     .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                     .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.status.ToString()))
                     .ForMember(dest => dest.DeliveryCost, opt => opt.MapFrom(src => src.DeliveryMethod.Cost))
                     .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.product.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.product.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());







        }
    }
}
