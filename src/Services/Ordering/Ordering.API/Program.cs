using Ordering.API;
using Ordering.Application;
using Ordering.Infastructure;
using Ordering.Infastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddApplicationServices()
                .AddInfastructureServices(builder.Configuration)
                .AddApiServices(builder.Configuration);


var app = builder.Build();

//Configure Http pipeline
app.UseApiServices();

if (app.Environment.IsDevelopment())
    await app.InitialiseDatabaseAsync();

app.Run();
