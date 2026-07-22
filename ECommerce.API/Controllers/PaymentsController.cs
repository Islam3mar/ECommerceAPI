using ECommerce.Application.Common;
using ECommerce.Application.DTO_s.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId ,CancellationToken ct)
        {
            var Result = await paymentService.CreateOrUpdatePaymentIntentAsync(basketId, ct);

            return ToActionResult(Result);
        }
    }
}
