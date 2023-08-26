using AutoMapper;
using MediatR;
using Ordering.Application.Contracts;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Queries.GetOrders
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<GetOrdersViewModel>>
    {
        private readonly IGenericRepository<Order> genericRepository;
        private readonly IMapper mapper;

        public GetOrdersQueryHandler(IGenericRepository<Order> genericRepository, IMapper mapper)
        {
            this.genericRepository = genericRepository;
            this.mapper = mapper;
        }
        public async Task<List<GetOrdersViewModel>> Handle(GetOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var orders = await genericRepository.GetallAsync();

            return mapper.Map<List<GetOrdersViewModel>>(orders);
        }
    }
}
