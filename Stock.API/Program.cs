using MassTransit;
using Stock.API.Models;
using Stock.API.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqps://ktzqjmzg:CtC9AkyO-89Pyg0h4ZEOafGNqlbVtI9i@puffin.rmq2.cloudamqp.com/ktzqjmzg");
    });
});

builder.Services.AddSingleton<MongoDbService>();

var app = builder.Build();

using var scope = builder.Services.BuildServiceProvider().CreateScope();

var mongoDbService = scope.ServiceProvider.GetRequiredService<MongoDbService>();

if(!await(await mongoDbService.GetCollection<StockEntity>().FindAsync(x => true)).AnyAsync())
{
    mongoDbService.GetCollection<StockEntity>().InsertOne(new StockEntity { Count = 100, ProductId = 1 });
    mongoDbService.GetCollection<StockEntity>().InsertOne(new StockEntity { Count = 200, ProductId = 2 });
    mongoDbService.GetCollection<StockEntity>().InsertOne(new StockEntity { Count = 300, ProductId = 3 });
    mongoDbService.GetCollection<StockEntity>().InsertOne(new StockEntity { Count = 400, ProductId = 4 });
    mongoDbService.GetCollection<StockEntity>().InsertOne(new StockEntity { Count = 500, ProductId = 5 });
}


app.Run();
