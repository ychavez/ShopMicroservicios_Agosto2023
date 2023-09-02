using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Commands.Checkout;

namespace Ordering.Api.EventBusConsumer
{
    public class EventConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public EventConsumer(IMapper mapper, IMediator mediator)
        {
            this.mapper = mapper;
            this.mediator = mediator;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
           var command = mapper.Map<CheckoutOrderCommand>(context.Message);
            _ = await mediator.Send(command);
        }
    }
}
