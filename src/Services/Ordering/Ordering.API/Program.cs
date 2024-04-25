using Ordering.API;
using Ordering.Application;
using Ordering.Infastructure;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddApplicationServices()
                .AddInfastructureServices(builder.Configuration)
                .AddApiServices();

var app = builder.Build();

//Configure Http pipeline


app.Run();
