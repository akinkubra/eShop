namespace eShop.Ordering.UnitTests.Application;

public class IdentifiedCommandHandlerTest
{
    private readonly IRequestManager _requestManager;
    private readonly IMediator _mediator;
    private readonly ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>> _loggerMock;
    private readonly ILogger<IdentifiedCommandHandler<CompleteOrderCommand, bool>> _loggerCompleteMock;

    public IdentifiedCommandHandlerTest()
    {
        _requestManager = Substitute.For<IRequestManager>();
        _mediator = Substitute.For<IMediator>();
        _loggerMock = Substitute.For<ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>>>();
        _loggerCompleteMock = Substitute.For<ILogger<IdentifiedCommandHandler<CompleteOrderCommand, bool>>>();
    }

    [Fact]
    public async Task Handler_sends_command_when_order_no_exists()
    {
        // Arrange
        var fakeGuid = Guid.NewGuid();
        var fakeOrderCmd = new IdentifiedCommand<CreateOrderCommand, bool>(FakeOrderRequest(), fakeGuid);

        _requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        _mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var handler = new CreateOrderIdentifiedCommandHandler(_mediator, _requestManager, _loggerMock);
        var result = await handler.Handle(fakeOrderCmd, CancellationToken.None);

        // Assert
        Assert.True(result);
        await _mediator.Received().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Fact]
    public async Task Handler_sends_no_command_when_order_already_exists()
    {
        // Arrange
        var fakeGuid = Guid.NewGuid();
        var fakeOrderCmd = new IdentifiedCommand<CreateOrderCommand, bool>(FakeOrderRequest(), fakeGuid);

        _requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(true));

        _mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var handler = new CreateOrderIdentifiedCommandHandler(_mediator, _requestManager, _loggerMock);
        var result = await handler.Handle(fakeOrderCmd, CancellationToken.None);

        // Assert
       await  _mediator.DidNotReceive().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Fact]
    public async Task Handler_sends_command_when_order_not_completed()
    {
        // Arrange
        var fakeGuid = Guid.NewGuid();
        var fakeOrderCompleteCmd = new IdentifiedCommand<CompleteOrderCommand, bool>(FakeOrderCompleteRequest(), fakeGuid);

        _requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        _mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var handler = new CompleteOrderIdentifiedCommandHandler(_mediator, _requestManager, _loggerCompleteMock);
        var result = await handler.Handle(fakeOrderCompleteCmd, CancellationToken.None);

        // Assert
        Assert.True(result);
        await _mediator.Received().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [Fact]
    public async Task Handler_sends_no_command_when_order_already_cmpleted()
    {
        // Arrange
        var fakeGuid = Guid.NewGuid();
        var fakeOrderCompletedCmd = new IdentifiedCommand<CompleteOrderCommand, bool>(FakeOrderCompleteRequest(), fakeGuid);

        _requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(true));

        _mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var handler = new CompleteOrderIdentifiedCommandHandler(_mediator, _requestManager, _loggerCompleteMock);
        var result = await handler.Handle(fakeOrderCompletedCmd, CancellationToken.None);

        // Assert
        await _mediator.DidNotReceive().Send(Arg.Any<IRequest<bool>>(), default);
    }


    private CreateOrderCommand FakeOrderRequest(Dictionary<string, object> args = null)
    {
        return new CreateOrderCommand(
            new List<BasketItem>(),
            userId: args != null && args.ContainsKey("userId") ? (string)args["userId"] : null,
            userName: args != null && args.ContainsKey("userName") ? (string)args["userName"] : null,
            city: args != null && args.ContainsKey("city") ? (string)args["city"] : null,
            street: args != null && args.ContainsKey("street") ? (string)args["street"] : null,
            state: args != null && args.ContainsKey("state") ? (string)args["state"] : null,
            country: args != null && args.ContainsKey("country") ? (string)args["country"] : null,
            zipcode: args != null && args.ContainsKey("zipcode") ? (string)args["zipcode"] : null,
            cardNumber: args != null && args.ContainsKey("cardNumber") ? (string)args["cardNumber"] : "1234",
            cardExpiration: args != null && args.ContainsKey("cardExpiration") ? (DateTime)args["cardExpiration"] : DateTime.MinValue,
            cardSecurityNumber: args != null && args.ContainsKey("cardSecurityNumber") ? (string)args["cardSecurityNumber"] : "123",
            cardHolderName: args != null && args.ContainsKey("cardHolderName") ? (string)args["cardHolderName"] : "XXX",
            cardTypeId: args != null && args.ContainsKey("cardTypeId") ? (int)args["cardTypeId"] : 0);
    }

    private CompleteOrderCommand FakeOrderCompleteRequest(Dictionary<string, object> args = null)
    {
        return new CompleteOrderCommand(
                       orderNumber: args != null && args.ContainsKey("orderNumber") ? (int)args["orderNumber"] : 1
                      );
    }
}
