using System;
using GreenPipes;
using InvoicingMicroservice.Core.Fluent;
using InvoicingMicroservice.Core.Interfaces.Services;
using InvoicingMicroservice.Core.Middlewares;
using InvoicingMicroservice.Core.Services;
using InvoicingMicroservice.Comunication.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Authentication;
using System.Collections.Generic;
using Comunication;

namespace InvoicingMicroservice
{
    public class Startup
    {
        private bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Authentication
            services.Configure<ApplicationOptions>(Configuration.GetSection("ApplicationOptions"));
            services.AddScoped<IHeaderContextService, HeaderContextService>();
            services.AddHttpContextAccessor();
            #endregion 

            services.AddDbContext<MicroserviceContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), builder => {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });

            #region MassTransit
            var rabbitMq = new RabbitMq();
            Configuration.GetSection("RabbitMq").Bind(rabbitMq);
            services.AddSingleton(rabbitMq);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(new Uri(rabbitMq.Host), h =>
                    {
                        h.Username(rabbitMq.Username);
                        h.Password(rabbitMq.Password);
                    });

                    config.ReceiveEndpoint("msinvo.product.queue", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<ProductConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();
            #endregion

            services.AddControllers();
            services.AddScoped<ErrorHandlingMiddleware>();

            services.AddAutoMapper(this.GetType().Assembly);

            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EDP-INVOICING-MSV", Version = "v1" });
            });
            #endregion

            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IMicroserviceService, MicroserviceService>();
            services.AddScoped<IProductService, ProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EDP-INVOICING-MSV");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
