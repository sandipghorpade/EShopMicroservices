using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Commands.CreateOrder;


public record CreateOrderCommand(OrderDto Order)
    :ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);


public class CreateCommandValidator: AbstractValidator<CreateOrderCommand>
{
    public CreateCommandValidator() {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("Customer Id is required");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("Order Items should not be empty");
    }
}