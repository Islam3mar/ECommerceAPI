using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Contracts;
using ECommerce.Application.Profiles;
using ECommerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application
{
    public static class ApplicationServicesRegistrations
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(c => c.AddProfile(new ProductProfile()),typeof(ApplicationServicesRegistrations).Assembly);
            services.AddScoped<IProductServices,ProductServices>();
            services.AddScoped<IBasketServices, BasketServices>();
            services.AddSingleton<ICacheServices, CacheServices>();

            services.AddScoped<IIdentityServices, IdentityServices>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<ITokenServices, TokenServices>();

            return services;
        }

        }
}
