using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_s.Basket;
using ECommerce.Application.DTO_s.Order;
using ECommerce.Application.Specifications;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;

namespace ECommerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IPaymentGateway paymentGateway;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly PaymentGatewaySettings _stripe;

        public PaymentService(IBasketRepository basketRepository,
                              IPaymentGateway paymentGateway,
                              IOptions<PaymentGatewaySettings> stripeSettings,
                              IUnitOfWork unitOfWork,
                              IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.paymentGateway = paymentGateway;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _stripe = stripeSettings.Value;
        }

        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {
            // 1.Validate Basket Found & Item

            var basket = await basketRepository.GetBasketAsyc(basketId, ct);

            if (basket is null)
                return Result<BasketDto>.Fail(Error.NotFound("Basket.NotFound", $"Basket With Id : {basketId} Is Not Found"));

            if (basket.Items.Count == 0)
                return Result<BasketDto>.Fail(Error.Validation("Basket.Empty", "Basket Is Empty"));

            //------------------------------------------------------------------------------------------------------------------

            // 2.Check If Product Is Exist Or Not

            var productRepo = unitOfWork.GetRepository<Product, int>();

            var productIds = basket.Items.Select(i => i.Id).ToHashSet(); // To Prevent Duplication

            var products = await productRepo.GetAllWithSpecificationsAsync(new ProductWithIdsSpecifications(productIds));

            foreach (var item in basket.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.Id);

                if (product is null)
                    return Result<BasketDto>.Fail(Error.NotFound("Product.NotFound", $"Product With Id : {item.Id} Is Not Found"));

                   // Price Of Prods
                   item.Price = product.Price;
            }
            //------------------------------------------------------------------------------------------------------------------

            // 3.Store Delivery Method
            var deliveryRepo = unitOfWork.GetRepository<DeliveryMethod, int>();

            var deliveryMethod = await deliveryRepo.GetByIdAsync(basket.DeliveryMethodId.Value, ct);

            if (deliveryMethod is null)
                return Result<BasketDto>.Fail(Error.NotFound("DeliveryMethod.NotFound", $"DeliveryMethod With Id : {basket.DeliveryMethodId.Value} Is Not Found"));

            // Cost Of Delivery
            basket.ShippingPrice = deliveryMethod.Cost;

            //------------------------------------------------------------------------------------------------------------------

            var subTotal = basket.Items.Sum(i => i.Quantity * i.Price);

            var amount = (long)Math.Round(subTotal + deliveryMethod.Cost * 100m);

            //------------------------------------------------------------------------------------------------------------------

            if (!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var result = await paymentGateway.UpdatePaymentIntentAsync(basket.PaymentIntentId, amount , ct);

                basket.PaymentIntentId = result.PaymentIntentId;
                basket.ClientSecret = result.ClientSecret;
            }
            else
            {
                var result = await paymentGateway.CreatePaymentIntentAsync(amount , _stripe.DefaultCurrency, ct);

                basket.PaymentIntentId = result.PaymentIntentId;
                basket.ClientSecret = result.ClientSecret;
            }

            await basketRepository.CreateOrUpdateBasketAsync(basket,ct:ct);

            return Result<BasketDto>.Ok(mapper.Map<BasketDto>(basket));

        }
    }

    public class PaymentGatewaySettings
    {
        public string SecretKey { get; set; } = default!;
        public string DefaultCurrency { get; set; } = "USD";
    }
}
