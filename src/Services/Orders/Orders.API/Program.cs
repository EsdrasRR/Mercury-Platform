using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Handlers;
using Orders.Application.Interfaces;
using Orders.Application.Mappings;
using Orders.Application.Messaging;
using Orders.Infrastructure;
using Orders.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrdersDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddMediatR(typeof(CreateOrderHandler).Assembly);

builder.Services.AddSingleton<IEventPublisher, NullEventPublisher>();

builder.Services.AddAutoMapper(typeof(OrderMappingProfile).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
