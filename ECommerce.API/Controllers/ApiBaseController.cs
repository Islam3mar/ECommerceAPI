using ECommerce.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        // for return Data
        public static ActionResult<T> ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result.data);
            
            return ToProblem(result.Errors);
        }
        // no data return
        public static ActionResult<T> ToActionResult<T>(Result result)
        {
            if (result.IsSuccess)
                return new OkResult();

            return ToProblem(result.Errors);
        }

        public static ObjectResult ToProblem(IReadOnlyList<Error> Errors)
        {
            var firstError = Errors[0];

            var status = firstError.errorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.UnAuthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            var promblem = new ProblemDetails
            {
                Status = status,
                Title = firstError.code,
                Detail = firstError.Description,
                Extensions = { ["errors"] = Errors }

            };

            return new ObjectResult(promblem)
            {
                StatusCode = status
            };
        }
    }
}
