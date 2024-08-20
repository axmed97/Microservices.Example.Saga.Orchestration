using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine.Service.StateDbContext;
using SagaStateMachine.Service.StateInstances;
using SagaStateMachine.Service.StateMachines;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
        .EntityFrameworkRepository(options =>
        {
            options.AddDbContext<DbContext, OrderStateDbContext>((provider, _builder) =>
            {
                _builder.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
        });
    configurator.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqps://ktzqjmzg:CtC9AkyO-89Pyg0h4ZEOafGNqlbVtI9i@puffin.rmq2.cloudamqp.com/ktzqjmzg");
    });
});

var host = builder.Build();
host.Run();
