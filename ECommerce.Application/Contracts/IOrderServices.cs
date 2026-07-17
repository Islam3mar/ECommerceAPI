using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Common;
using ECommerce.Application.DTO_s.Order;

namespace ECommerce.Application.Contracts
{
    public interface IOrderServices
    {
        Task<Result<OrderToReturnDto>> CreateOrdersAsync(OrderDto orderDto, string email, CancellationToken ct = default);

        Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync(CancellationToken ct = default);
        Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailAsync(Guid Id,string email, CancellationToken ct = default);



    }
}
