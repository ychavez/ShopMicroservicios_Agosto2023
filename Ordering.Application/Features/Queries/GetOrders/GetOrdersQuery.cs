using MediatR;

namespace Ordering.Application.Features.Queries.GetOrders
{
    public class GetOrdersQuery: IRequest<List<GetOrdersViewModel>>
    {
    }
}
