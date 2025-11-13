using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Application.Handlers;
using Orders.Application.Mappings;
using Orders.Application.Messaging;
using Orders.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core
builder.Services.AddDbContext<OrdersDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Repositories / DI
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// MediatR - register handlers from the assembly that contains CreateOrderHandler
builder.Services.AddMediatR(typeof(CreateOrderHandler).Assembly);

// Event publishing (fallback)  
builder.Services.AddSingleton<IEventPublisher, NullEventPublisher>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(OrderMappingProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
