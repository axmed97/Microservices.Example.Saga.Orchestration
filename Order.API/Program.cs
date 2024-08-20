using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.DTOs;
using Order.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqps://ktzqjmzg:CtC9AkyO-89Pyg0h4ZEOafGNqlbVtI9i@puffin.rmq2.cloudamqp.com/ktzqjmzg");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/create-order", async (CreateOrderDto model, AppDbContext context) =>
{
    OrderEntity orderEntity = new()
    {
        BuyerId = model.BuyerId,
        CreatedDate = DateTime.Now,
        OrderStatus = Order.API.Enums.OrderStatus.Suspend,
        TotalPrice = model.OrderItemDtos.Sum(x => x.Price * x.Count),
        OrderItems = model.OrderItemDtos.Select(x => new OrderItem
        {
            Count = x.Count,
            Price = x.Price,
            ProductId = x.ProductId,
        }).ToList()
    };

    await context.OrderEntities.AddAsync(orderEntity);
    await context.SaveChangesAsync();
});

app.Run();


