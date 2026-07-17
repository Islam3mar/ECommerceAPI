using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_s.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class OrderController : ApiBaseController
    {
        private readonly IOrderServices orderServices;

        public OrderController(IOrderServices orderServices)
        {
            this.orderServices = orderServices;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder([FromBody] OrderDto orderDto,
                                                                [FromQuery] string email,
                                                                CancellationToken ct)
        {
            return ToActionResult(await orderServices.CreateOrdersAsync(orderDto, email, ct));
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethods(CancellationToken ct)
        {
            return ToActionResult(await orderServices.GetAllDeliveryMethodsAsync(ct));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersByEmail([FromQuery]string email, CancellationToken ct)
        {
            return ToActionResult(await orderServices.GetAllOrdersByEmailAsync(email,ct));
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrdersByIdAndEmail(Guid id,[FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await orderServices.GetOrderByIdAndEmailAsync(id,email, ct));
        }
    }
}
