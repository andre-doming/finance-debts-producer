using finance.debts.producer;
using finance.debts.producer.Domain.Debts;
using finance.debts.producer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "finance", h =>
        {
            h.Username("svc-debts");
            h.Password("SvcDebts!7pQ3a");
        });

        cfg.Message<DebtCreatedEvent>(e =>
        {
            e.SetEntityName("finance.debts.created");
        });
    });
});

var host = builder.Build();
host.Run();
