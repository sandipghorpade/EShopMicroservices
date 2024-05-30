using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Weasel.Core;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Discount.Grpc;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

//Add Services to Container

//Application Services 
builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

//Data Services
builder.Services.AddMarten(options => {
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x=>x.UserName);
}).UseLightweightSessions();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName="Basket";
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

//register Discount.grpc 
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
    options =>
    {
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
    }).ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =HttpClientHandler.
                                                            DangerousAcceptAnyServerCertificateValidator
        };
        return handler;
    });

//Cross Cutting
//Exception Hnadling service
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

//Health Check service
builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
                .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

//Configure the Http Request pipeline
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }); ;

app.Run();
