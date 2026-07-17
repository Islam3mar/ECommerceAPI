using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ECommerce.Application.DTO_s.Order;
using ECommerce.Application.DTO_s.Products;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;

namespace ECommerce.Application.Profiles
{
    public class OrderItemPictureUrlResolver(IOptions<UrlSettings> options) : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly UrlSettings settings = options.Value;
        public string? Resolve(OrderItem source, OrderItemDto destination, string? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.product.PictureUrl))
                return string.Empty;

            return $"{settings.BaseUrl}/Files/{source.product.PictureUrl}";
        }
    }
}
