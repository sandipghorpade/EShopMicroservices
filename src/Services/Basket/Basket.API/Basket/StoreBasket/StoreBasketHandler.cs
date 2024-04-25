

using Basket.API.Data;
using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart): ICommand<StoreBasketResult>;

    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotEmpty().WithMessage("Cart Cannot be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class StoreBasketCommandHandler(IBasketRepository basketRepository, 
                                           DiscountProtoService.DiscountProtoServiceClient discountProto) 
                                           : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, 
                                                    CancellationToken cancellationToken)
        {
            //ToDo: conunicate with Discount.grpc to calculate latest price of product
            await DeductDiscount(command.Cart, cancellationToken);
            await basketRepository.StoreBasket(command.Cart, cancellationToken);
            return new StoreBasketResult(command.Cart.UserName);
           
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var copoun = await discountProto.GetDiscountAsync(
                                                new GetDiscountRequest { ProductName = item.ProductName },
                                                cancellationToken: cancellationToken);
                item.Price -= copoun.Amount;
            }
        }
    }
}
