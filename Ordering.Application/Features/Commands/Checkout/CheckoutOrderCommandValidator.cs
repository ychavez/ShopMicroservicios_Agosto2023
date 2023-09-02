using FluentValidation;

namespace Ordering.Application.Features.Commands.Checkout
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(x => x.Address)
                .EmailAddress()
                .NotEmpty();

            RuleFor(x => x.TotalPrice).GreaterThan(0);

            RuleFor(x => x.UserName)
                .MinimumLength(4)
                .WithMessage("El usuario no puede ser menor a 4 caracteres, no manche!")
                .MaximumLength(20);
        }

    }
}
