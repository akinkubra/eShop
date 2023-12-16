namespace eShop.Ordering.API.Application.Validations
{
    public class CompleteOrderCommandValidator: AbstractValidator<CompleteOrderCommand>
    {
        public CompleteOrderCommandValidator(ILogger<CompleteOrderCommandValidator> logger)
        {
            RuleFor(order => order.OrderNumber).NotEmpty().WithMessage("No orderId found");
            RuleFor(order => order.OrderNumber).GreaterThanOrEqualTo(1).WithMessage("Order number is invalid");

            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("INSTANCE CREATED - {ClassName}", GetType().Name);
            }
        }
    }
}
