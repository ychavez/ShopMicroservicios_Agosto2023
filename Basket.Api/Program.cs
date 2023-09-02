
using Basket.Api.Repositories;
using Inventory.grpc.Protos;
using MassTransit;

namespace Basket.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddStackExchangeRedisCache(x =>
            {
                x.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            builder.Services.AddGrpcClient<ExistenceService.ExistenceServiceClient>
                (x=> x.Address = new Uri(builder.Configuration["GrpcSettings:HostAddress"]!));

            builder.Services.AddMassTransit(x =>
            x.UsingRabbitMq((ctx, cfg) => cfg.Host(builder.Configuration["EventBusSettings:HostAddress"])));

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}