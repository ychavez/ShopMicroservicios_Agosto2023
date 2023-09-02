using FluentValidation;
using MediatR;

namespace Ordering.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            if (validators.Any())
            {
                // nos traemos el contexto de las validaciones
                var context = new ValidationContext<TRequest>(request);

                // ejecutamos las validaciones 
                var validationResults = await Task.WhenAll(
                    validators.Select(v => v.ValidateAsync(context)));   

                // buscar errores
                var failures = validationResults.SelectMany(r => r.Errors)
                    .Where(f=> f is not null).ToList();

                // regresamos los errores
                if (failures.Any())
                    throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
