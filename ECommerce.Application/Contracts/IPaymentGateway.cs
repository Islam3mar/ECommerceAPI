using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Common;

namespace ECommerce.Application.Contracts
{
    public interface IPaymentGateway
    {
        Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount,string currency,CancellationToken ct = default);
        Task<PaymentIntentResult> UpdatePaymentIntentAsync(string paymentIntentId , decimal amount, CancellationToken ct = default);

    }
}
