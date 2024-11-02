using NbusRabbitMQWebApi.Commands;
using NbusRabbitMQWebApi.Configs;
using NbusRabbitMQWebApi.Data;
using NbusRabbitMQWebApi.Handlers;
using NbusRabbitMQWebApi.Repositorys;
using NbusRabbitMQWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Build the service provider and assign it to ServiceLocator
ServiceLocator.ServiceProvider = builder.Services.BuildServiceProvider();

builder.Services.AddTransient<IHandleMessages<PlaceOrderCommand>, PlaceOrderCommandHandler>();
builder.Services.AddSingleton<IMessageSession>(provider => NServiceBusConfig.StartEndpointAsync().GetAwaiter().GetResult());




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

