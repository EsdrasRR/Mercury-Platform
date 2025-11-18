using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Payments.Infrastructure.PaymentsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PaymentsConnection")));

builder.Services.AddScoped<Payments.Application.Interfaces.IPaymentRepository, Payments.Infrastructure.Repositories.PaymentRepository>();

builder.Services.AddAutoMapper(typeof(Payments.Application.Mapping.PaymentProfile).Assembly);
builder.Services.AddMediatR(typeof(CreatePaymentCommand).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/payments", async (Payments.Application.Commands.CreatePaymentCommand cmd, IMediator mediator) =>
{
    var created = await mediator.Send(cmd);
    return Results.Created($"/api/payments/{created.Id}", created);
});

app.MapGet("/api/payments/{id:guid}", async (Guid id, Payments.Application.Interfaces.IPaymentRepository repo, IMapper mapper) =>
{
    var p = await repo.GetByIdAsync(id);
    return p is null ? Results.NotFound() : Results.Ok(mapper.Map<Payments.Application.DTOs.PaymentDto>(p));
});

app.Run();
