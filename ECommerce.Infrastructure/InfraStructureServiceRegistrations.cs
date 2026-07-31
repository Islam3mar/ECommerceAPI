using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Contracts;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Payments;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Seeding;
using ECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace ECommerce.Infrastructure
{
    public static class InfraStructureServiceRegistrations
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // connection string DbStore
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("StoreDbConnection"));
            });

            // connection string DbIdentity
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog"); // Register the CatalogDataSeeder with a key "Catalog" Because later in another class he will need to Impelement The same interface
            services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>("Identity"); 


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IConnectionMultiplexer>(opt =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
            });

            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddSingleton<ICacheRepository, CacheRepository>();


            services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = true;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = configuration.GetSection("JWT")["Issuer"],
                    ValidAudience = configuration.GetSection("JWT")["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT")["SecretKey"]))
                };
            });

            services.AddScoped<IPaymentGateway,StripePaymentGateway>();


            services.AddScoped<IImageService, ImageService>();

            return services;
        }
    }
}