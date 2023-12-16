namespace eShop.Ordering.API.Application.Commands
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CompleteOrderCommandHandler> _logger;

        public CompleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<CompleteOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handler which processes the command when
        /// administrator executes ship order from app
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CompleteOrderCommand command, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
            if (orderToUpdate == null)
            {
                return false;
            }

            orderToUpdate.SetCompletedStatus();

            _logger.LogInformation("Change Order Status - Order: {@Order}", orderToUpdate);

            return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}

// Use for Idempotency in Command process
public class CompleteOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CompleteOrderCommand, bool>
{
    ILogger<IdentifiedCommandHandler<CompleteOrderCommand, bool>> logger;
    public CompleteOrderIdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<CompleteOrderCommand, bool>> logger)
        : base(mediator, requestManager, logger)
    {
        this.logger = logger;
    }

    protected override bool CreateResultForDuplicateRequest()
    {
        logger.LogWarning("this request already sended");
        return true; // Ignore duplicate requests for processing order.
    }
}
