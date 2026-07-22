using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.Services;
using Microsoft.Extensions.Options;
using Stripe;

namespace ECommerce.Infrastructure.Payments
{
    public class StripePaymentGateway : IPaymentGateway
    {
        private readonly PaymentIntentService _paymentIntentService = new();

        public StripePaymentGateway(IOptions<PaymentGatewaySettings> options)
        {
            StripeConfiguration.ApiKey = options.Value.SecretKey;
        }
        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            var IntenetOptions = new PaymentIntentCreateOptions
            {
                Amount = (long)amount,
                Currency = currency.ToLower(),
                PaymentMethodTypes = ["card"]
            };

            var intent = await _paymentIntentService.CreateAsync(IntenetOptions,cancellationToken: ct);

            return new PaymentIntentResult(intent.Id, intent.ClientSecret);
        }

        public async  Task<PaymentIntentResult> UpdatePaymentIntentAsync(string paymentIntentId, decimal amount, CancellationToken ct = default)
        {
            var IntenetOptions = new PaymentIntentUpdateOptions
            {
                Amount = (long)amount,
          
            };

            var intent = await _paymentIntentService.UpdateAsync(paymentIntentId,IntenetOptions, cancellationToken: ct);

            return new PaymentIntentResult(intent.Id, intent.ClientSecret);

        }
    }


}
