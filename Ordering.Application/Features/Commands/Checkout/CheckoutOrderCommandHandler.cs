using AutoMapper;
using MediatR;
using Ordering.Application.Contracts;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Commands.Checkout
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IGenericRepository<Order> genericRepository;
        private readonly IMapper mapper;

        public CheckoutOrderCommandHandler(IGenericRepository<Order> genericRepository, 
            IMapper mapper)
        {
            this.genericRepository = genericRepository;
            this.mapper = mapper;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var order = mapper.Map<Order>(request);

            var newOrder = await genericRepository.AddAsync(order);

            return newOrder.Id;
        }
    }
}
