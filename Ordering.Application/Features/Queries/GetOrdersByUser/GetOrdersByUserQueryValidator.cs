using FluentValidation;

namespace Ordering.Application.Features.Queries.GetOrdersByUser
{
    public class GetOrdersByUserQueryValidator : AbstractValidator<GetOrdersByUserQuery>
    {
        public GetOrdersByUserQueryValidator()
        {
            RuleFor(x => x.Username).NotEmpty()
                .MinimumLength(4)
                .MaximumLength(20);
        }
    }
}
