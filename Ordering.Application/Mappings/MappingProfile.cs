using AutoMapper;
using Ordering.Application.Features.Commands.Checkout;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
        }
    }
}
