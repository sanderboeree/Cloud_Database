using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Quartz;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using OnlineStore.Api.Application.Users;
using OnlineStore.Api.Application.Users.Interfaces;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.Configuration;
using OnlineStore.Api.Infrastructure.EntityFramework;
using OnlineStore.Api.Infrastructure.EntityFramework.Data;
using OnlineStore.Api.Infrastructure.Events;
using OnlineStore.Api.Infrastructure.Events.Interfaces;
using OnlineStore.Api.Infrastructure.ExceptionHandlers.GlobalExceptionHandler;
using OnlineStore.Api.Infrastructure.ExceptionHandlers.NotFoundExceptionHandler;
using OnlineStore.Api.Infrastructure.ExceptionHandlers.UpdateExceptionHandler;
using OnlineStore.Api.Infrastructure.ExceptionHandlers.ValidationExceptionHandler;
using OnlineStore.Api.Infrastructure.Extensions;
using OnlineStore.Api.Infrastructure.Identity;
using OnlineStore.Api.Infrastructure.Identity.Interfaces;
using OnlineStore.Api.Infrastructure.Json;
using OnlineStore.Api.Infrastructure.ModelBinding;
using OnlineStore.Api.Infrastructure.Repositories;
using OnlineStore.Api.Infrastructure.Repositories.Interfaces;
using OnlineStore.Api.Infrastructure.SwashBuckle;
using OnlineStore.Api.Application.Orders;
using OnlineStore.Api.Infrastructure.Crud;
using OnlineStore.Api.Infrastructure.Specifications;
using OnlineStore.Api.Infrastructure.Crud.Interfaces;
using OnlineStore.Api.Infrastructure.Azure;
using OnlineStore.Api.Infrastructure.Azure.Interfaces;
using OnlineStore.Api.Application.Orders.Interfaces;

namespace OnlineStore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("OnlineStore"),
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null)
                .UseNetTopologySuite()
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            services.TryAddScoped(typeof(IRepository<>), typeof(Repository<>));

            AddCrudServices(services);
            AddServices(services);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

            services
                .AddIdentity<User, Role>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var tokenSection = Configuration.GetSection("Token");
            services.Configure<TokenSettings>(tokenSection);
            var jwtBearerTokenSettings = tokenSection.Get<TokenSettings>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtBearerTokenSettings.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtBearerTokenSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBearerTokenSettings.SecretKey)),
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.TryAddSingleton(tokenValidationParameters);
            services.TryAddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(RoleData.Admin, policy => policy.RequireAssertion(context => context.User.HasClaim(RoleData.Admin)));
            });

            services
                .AddApplicationInsightsTelemetry(options => options.EnableAdaptiveSampling = false)
                .AddCors(options =>
                {
                    options.AddPolicy("CORS", builder =>
                    {
                        builder
                            .SetIsOriginAllowed(_ => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithExposedHeaders("content-disposition");
                    });
                })
                .AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>())
                .AddSwaggerGen()
                .AddSwaggerGenNewtonsoftSupport()
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                })
                .AddControllers(options =>
                {
                    options.ModelBinderProviders.Insert(0, new OnlineStoreEnumModelBinderProvider());
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(new OnlineStoreEnumNamingStrategy()));
                })
                .AddFluentValidation(options =>
                {
                    options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    options.RegisterValidatorsFromAssemblyContaining<Startup>();
                });

            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(Configuration.GetConnectionString("AzureStorage"))
                    .ConfigureOptions(options =>
                     {
                         options.Retry.Mode = Azure.Core.RetryMode.Exponential;
                         options.Retry.MaxRetries = 5;
                         options.Retry.MaxDelay = TimeSpan.FromSeconds(30);
                     });
                builder.AddQueueServiceClient(Configuration.GetConnectionString("AzureStorage"))
                .ConfigureOptions(options =>
                {
                    options.Retry.Mode = Azure.Core.RetryMode.Exponential;
                    options.Retry.MaxRetries = 5;
                    options.Retry.MaxDelay = TimeSpan.FromSeconds(30);
                });
            });
        }

        private static void AddServices(IServiceCollection services)
        {
            services.TryAddScoped<IUserAuthenticator, UserAuthenticator>();
            services.TryAddScoped<ITokenService, TokenService>();
            services.TryAddScoped<IUserActivator, UserActivator>();
            services.TryAddScoped<IBlobStorage, BlobStorage>();
            services.TryAddScoped<IQueueStorage, QueueStorage>();
            services.TryAddScoped<IProductImageFileService, ProductImageFileService>();
            services.TryAddScoped<IOrderQueueService, OrderQueueService>();

            services.TryAddSingleton<IEventDispatcher, EventDispatcher>();
            services.TryAddSingleton<IUser, OnlineStoreUser>();
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
        }

        private static void AddCrudServices(IServiceCollection services)
        {
            services.TryAddScoped<ICrudService<Order, OrderDto>, CrudService<Order, OrderDto, OrderWithId>>();
            services.TryAddScoped<ICrudService<Product, ProductDto>, CrudService<Product, ProductDto, ProductWithId>>();
            services.TryAddScoped<ICrudService<OrderProduct, OrderProductDto>, CrudService<OrderProduct, OrderProductDto, WithId<OrderProduct>>>();
            services.TryAddScoped<ICrudService<Image, ImageDto>, CrudService<Image, ImageDto, WithId<Image>>>();
            services.TryAddScoped<ICrudService<Address, AddressDto>, CrudService<Address, AddressDto, WithId<Address>>>();
            services.TryAddScoped<IUserCrudService, UserCrudService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CORS");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseOnlineStoreGlobalExceptionHandler();
                app.UseHsts();
            }

            app.UseOnlineStoreNotFoundExceptionHandler();
            app.UseOnlineStoreUpdateExceptionHandler();
            app.UseOnlineStoreValidationExceptionHandler();

            app
                .UseHttpsRedirection()
                .UseResponseCompression()
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpper());
                    }
                })
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
