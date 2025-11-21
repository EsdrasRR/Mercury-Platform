using AutoMapper;
using Moq;
using Payments.Application.Commands;
using Payments.Application.DTOs;
using Payments.Application.Handlers;
using Payments.Application.Interfaces;
using Payments.Domain.Entities;

namespace Payments.UnitTests.Handlers;

public class CreatePaymentHandlerTests
{
    private readonly Mock<IPaymentRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreatePaymentHandler _handler;

    public CreatePaymentHandlerTests()
    {
        _mockRepository = new Mock<IPaymentRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreatePaymentHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreatePaymentAndReturnDto()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var amount = 100.50m;
        var currency = "AUD";

        var command = new CreatePaymentCommand(orderId, amount, currency);

        var expectedDto = new PaymentDto
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Amount = amount,
            Currency = currency,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockRepository
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockMapper
            .Setup(m => m.Map<PaymentDto>(It.IsAny<Payment>()))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(amount, result.Amount);
        Assert.Equal(currency, result.Currency);
        Assert.Equal("Pending", result.Status);

        _mockRepository.Verify(r => r.AddAsync(
            It.Is<Payment>(p =>
                p.OrderId == orderId &&
                p.Amount == amount &&
                p.Currency == currency),
            It.IsAny<CancellationToken>()),
            Times.Once);

        _mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockMapper.Verify(m => m.Map<PaymentDto>(It.IsAny<Payment>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCallRepositoryMethodsInCorrectOrder()
    {
        // Arrange
        var command = new CreatePaymentCommand(Guid.NewGuid(), 50.00m, "USD");

        var callSequence = new List<string>();

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .Callback(() => callSequence.Add("AddAsync"))
            .Returns(Task.CompletedTask);

        _mockRepository
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callSequence.Add("SaveChangesAsync"))
            .Returns(Task.CompletedTask);

        _mockMapper
            .Setup(m => m.Map<PaymentDto>(It.IsAny<Payment>()))
            .Returns(new PaymentDto());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(2, callSequence.Count);
        Assert.Equal("AddAsync", callSequence[0]);
        Assert.Equal("SaveChangesAsync", callSequence[1]);
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var command = new CreatePaymentCommand(Guid.NewGuid(), 75.00m, "EUR");

        var cancellationToken = new CancellationToken();

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Payment>(), cancellationToken))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mockRepository
            .Setup(r => r.SaveChangesAsync(cancellationToken))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mockMapper
            .Setup(m => m.Map<PaymentDto>(It.IsAny<Payment>()))
            .Returns(new PaymentDto());

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Payment>(), cancellationToken), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_DefaultCurrency_ShouldUseAUD()
    {
        // Arrange
        var command = new CreatePaymentCommand(Guid.NewGuid(), 200.00m, "AUD");

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockRepository
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockMapper
            .Setup(m => m.Map<PaymentDto>(It.IsAny<Payment>()))
            .Returns(new PaymentDto { Currency = "AUD" });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(
            It.Is<Payment>(p => p.Currency == "AUD"),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_MapperReturnsDifferentDto_ShouldReturnMappedDto()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new CreatePaymentCommand(orderId, 150.00m, "GBP");

        var expectedDto = new PaymentDto
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Amount = 150.00m,
            Currency = "GBP",
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            ExternalPaymentId = null,
            ErrorMessage = null
        };

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockRepository
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockMapper
            .Setup(m => m.Map<PaymentDto>(It.IsAny<Payment>()))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Same(expectedDto, result);
    }
}
