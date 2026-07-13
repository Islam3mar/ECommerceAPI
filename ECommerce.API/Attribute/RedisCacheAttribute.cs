using System.Text;
using ECommerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.API.Attribute
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationInSeconds;

        public RedisCacheAttribute(int DurationInSeconds)
        {
            _durationInSeconds = DurationInSeconds;
        }


        // context => Request , Response
        // next => delegate refere to next Running Code
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get Cache Service From DI Container
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheServices>();

            var cacheKey = CreateCacheKey(context.HttpContext.Request);

            // Check If Cached Data Exsists
            var Cached = await cacheService.GetAsync(cacheKey);
            // If Exsists => Return Cached Data And Skip Execution End Point
            if (!string.IsNullOrEmpty(Cached))
            {
                context.Result = new ContentResult()
                {
                    Content = Cached,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return; // => Don't Read End Point
            }
            // If Not Exsists => Excute The End Point And Cach The Data And Return Response

            var Excuted = await next.Invoke();

            if (Excuted.Result is OkObjectResult { Value: not null } ok)
                await cacheService.SetAsync(cacheKey, ok.Value, TimeSpan.FromSeconds(_durationInSeconds));

            return;
        }
        private static string CreateCacheKey(HttpRequest request)
        {
            // path
            // api/Product?
            // api/Product?brandId = 10

            var key = new StringBuilder();
            key.Append(request.Path).Append("?");

            // key : brandId
            // value : 10
            foreach (var (k, v) in request.Query.OrderBy(q => q.Key))
            {
                key.Append(k).Append("=").Append(v).Append("&");
            }

            return key.ToString();
        }

    }
}
