using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_s.Order;
using ECommerce.Application.Specifications;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IMapper mapper;
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderServices(IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<OrderToReturnDto>> CreateOrdersAsync(OrderDto orderDto, string email, CancellationToken ct = default)
        {
            // 1.Validate Basket Found & Item

            var basket = await basketRepository.GetBasketAsyc(orderDto.BasketId, ct);

            if (basket is null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Basket.NotFound", $"Basket With Id : {orderDto.BasketId} Is Not Found"));

            if (basket.Items.Count == 0)
                return Result<OrderToReturnDto>.Fail(Error.Validation("Basket.Empty", "Basket Is Empty"));

            //----------------------------------------------------------------------------------------------------------------

            // 2.Get Items From Basket To Validate As Product 
            // Then Get Data From Product => Make It AS OrderItem

            var productRepo = unitOfWork.GetRepository<Product, int>();

            var productIds = basket.Items.Select(i => i.Id).ToHashSet(); // To Prevent Duplication

            var products = await productRepo.GetAllWithSpecificationsAsync(new ProductWithIdsSpecifications(productIds));

            var orderItems = new List<OrderItem>(basket.Items.Count);

            foreach (var item in basket.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.Id);

                if (product is null)
                    return Result<OrderToReturnDto>.Fail(Error.NotFound("Product.NotFound", $"Product With Id : {item.Id} Is Not Found"));

                orderItems.Add(new OrderItem()
                {
                    Price = product.Price,
                    Quantity = item.Quantity,

                    product = new ProductItemOrdered()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl,
                    }
                });
            }

            //------------------------------------------------------------------------------------------------------------------
            // 3.Store Order Address

            var orderAddress = mapper.Map<OrderAddress>(orderDto.ShippingAddress);

            //------------------------------------------------------------------------------------------------------------------
            // 4.Store Delivery Method

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId, ct);

            if (deliveryMethod is null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("DeliveryMethod.NotFound", $"DeliveryMethod With Id : {orderDto.DeliveryMethodId} Is Not Found"));

            //-------------------------------------------------------------------------------------------------------------------
            // 5.Calculations

            var subTotal = orderItems.Sum(oi => oi.Price * oi.Quantity);
            //-------------------------------------------------------------------------------------------------------------------
            // 6.Generate Order

            var order = new Order()
            {
                BuyerEmail = email,
                Items = orderItems,
                ShippingAddress = orderAddress,
                DeliveryMethodId = deliveryMethod.Id,
                DeliveryMethod = deliveryMethod,
                SubTotal = subTotal,

            };

            unitOfWork.GetRepository<Order, Guid>().Add(order);

            var result = await unitOfWork.SaveChangesAsync();

            //--------------------------------------------------------------------------------------------------------------------
            // 7.Return Order
          
            if(result <= 0)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Order.Failure","Order Can Not Created"));

            await basketRepository.DeleteBasketAsync(orderDto.BasketId, ct);

            return Result<OrderToReturnDto>.Ok(mapper.Map<OrderToReturnDto>(order));
        }

        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync(CancellationToken ct = default)
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync(ct);

            return Result<IReadOnlyList<DeliveryMethodDto>>
                    .Ok(mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethods));
        }
        public async Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersByEmailAsync(string email, CancellationToken ct = default)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllWithSpecificationsAsync(new OrderSpecifications(email));

            return Result<IReadOnlyList<OrderToReturnDto>>
                  .Ok(mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }

        public async Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailAsync(Guid Id, string email, CancellationToken ct = default)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>().GetByIdWithSpecificationsAsync(new OrderSpecifications(Id, email));

            if(order is null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Order.NotFound", $"Order With Id : {Id} Is Not Found"));


            return Result<OrderToReturnDto>
                  .Ok(mapper.Map<OrderToReturnDto>(order));
        }
    }
}
