using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Commands.Checkout;
using Ordering.Application.Features.Queries.GetOrders;
using Ordering.Application.Features.Queries.GetOrdersByUser;

namespace Ordering.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder([FromBody] CheckoutOrderCommand command)
            => await mediator.Send(command);

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<GetOrdersViewModel>>> GetOrders()
         => await mediator.Send(new GetOrdersQuery());

        [HttpGet("username")]
        public async Task<ActionResult<List<GetOrdersViewModel>>> GetOrders(string username)
        => await mediator.Send(new GetOrdersByUserQuery() { Username = username });
    }
}
