using ApiClient.Application.Interfaces;
using ApiClient.Application.Services;
using Application.ServiceCollectionExtension;
using Asp.Versioning;
using Infrastructure.ServiceCollectionExtension;
using Messaging.Application.ServiceCollectionExtension;
using Orders.Application.ServiceCollectionExtension;
using Orders.Infrastructure.ServiceCollectionExtension;
using MailSender.Application.ServiceCollectionExtension;
using Orders.Presentation.Controller.v1;
using Presentation.Controller.v1;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

#region DI

builder.Services.AddInventoryModuleInfrastructure(builder.Configuration);
builder.Services.AddOrderModuleInfrastructure(builder.Configuration);
builder.Services.AddProductServices(builder.Configuration);
builder.Services.AddOrderServices(builder.Configuration);
builder.Services.AddMessageQueueServices(builder.Configuration);
builder.Services.AddEmailServices(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IApiClient, HttpApiClient>();
#endregion


builder.Services.AddControllers()
    .AddApplicationPart(typeof(ProductController).Assembly)
    .AddApplicationPart(typeof(OrderController).Assembly); ;
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

#region API Versioning

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // Set a default version
    options.AssumeDefaultVersionWhenUnspecified = true; // Use default if no version is specified
    options.ReportApiVersions = true; // Include API versions in response headers
});

#endregion

#region Swagger
builder.Services.AddSwaggerGen();
#endregion


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
