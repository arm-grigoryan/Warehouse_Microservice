using FluentValidation;
using FluentValidation.AspNetCore;
using Warehouse.API.Middlewares;
using Warehouse.API.Validations.Orders;
using Warehouse.ApplicationCore;
using Warehouse.Infrastructure;
using Warehouse.Persistence;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.

services.AddControllers();
services.AddFluentValidationAutoValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

services.AddAutoMapper(typeof(Program));

services.AddCoreServices()
        .AddInfrastructureServices(configuration)
        .AddPersistenceServices(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
