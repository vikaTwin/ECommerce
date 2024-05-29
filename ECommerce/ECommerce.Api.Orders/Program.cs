using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Profiles;
using ECommerce.Api.Orders.Providers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    options.UseInMemoryDatabase("Orders");
});

builder.Services.AddAutoMapper(typeof(OrdersProfile));
builder.Services.AddScoped<IOrdersProvider, OrdersProvider>();

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
