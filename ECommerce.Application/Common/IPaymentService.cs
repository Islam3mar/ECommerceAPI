using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTO_s.Basket;

namespace ECommerce.Application.Common
{
    public interface IPaymentService
    {
        Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default);
    }
}
