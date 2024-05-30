using FluentValidation;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(OrderDto Order):ICommand<UpdateOrderResult>
    {
    }

    public record UpdateOrderResult(bool IsSuccess);


    public class UpateCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpateCommandValidator()
        {
            RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("Customer Id is required");
            RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("Order Items should not be empty");
        }
    }
}
