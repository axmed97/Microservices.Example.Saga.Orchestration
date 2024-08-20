using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqps://ktzqjmzg:CtC9AkyO-89Pyg0h4ZEOafGNqlbVtI9i@puffin.rmq2.cloudamqp.com/ktzqjmzg");
    });
});

var host = builder.Build();
host.Run();
