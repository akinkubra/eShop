namespace eShop.WebApp.Services.OrderStatus.IntegrationEvents;
public class OrderStatusChangedToCompletedIntegrationEventHandler(
    OrderStatusNotificationService orderStatusNotificationService,
    ILogger<OrderStatusChangedToCompletedIntegrationEventHandler> logger)
    : IIntegrationEventHandler<OrderStatusChangedToCompletedIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToCompletedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);
        await orderStatusNotificationService.NotifyOrderStatusChangedAsync(@event.BuyerIdentityGuid);
    }
}

