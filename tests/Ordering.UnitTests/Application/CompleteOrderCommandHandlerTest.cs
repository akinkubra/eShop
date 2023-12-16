using eShop.Ordering.API.Application.IntegrationEvents;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

using NSubstitute;

namespace eShop.Ordering.UnitTests.Application;

public class CompeteOrderRequestHandlerTest
{
    private readonly IOrderRepository _orderRepositoryMock;

    public CompeteOrderRequestHandlerTest()
    {
        _orderRepositoryMock = Substitute.For<IOrderRepository>();
    }


    [Fact]
    public async Task Handle_return_true_if_order_is_available()
    {
        _orderRepositoryMock.GetAsync(Arg.Any<int>())
           .Returns(Task.FromResult(FakeOrder()));

        _orderRepositoryMock.UnitOfWork.SaveEntitiesAsync(default)
            .Returns(Task.FromResult(true));

        var loggerMock = Substitute.For<ILogger<CompleteOrderCommandHandler>>();

        var handler = new CompleteOrderCommandHandler(_orderRepositoryMock, loggerMock);
        var cltToken = new CancellationToken();
        var result = await handler.Handle(FakeCompleteOrderCommand(), cltToken);

        Assert.True(result);
    }

    [Fact]
    public async Task Handle_return_false_if_order_notfound()
    {
        _orderRepositoryMock.GetAsync(Arg.Any<int>())
           .Returns(Task.FromResult(FakeNullOrder()));

        var loggerMock = Substitute.For<ILogger<CompleteOrderCommandHandler>>();

        var handler = new CompleteOrderCommandHandler(_orderRepositoryMock,loggerMock);
        var cltToken = new CancellationToken();
        var result = await handler.Handle(FakeCompleteOrderCommand(), cltToken);

        Assert.False(result);
    }
   
    private Order FakeOrder()
    {
        return new Order("1", "fakeName", new Address("street", "city", "state", "country", "zipcode"), 1, "12", "111", "fakeName", DateTime.UtcNow.AddYears(1));
    }

    private Order FakeNullOrder()
    {
        return null;
    }

    private CompleteOrderCommand FakeCompleteOrderCommand()
    {
        return new CompleteOrderCommand(1);
    }
}
