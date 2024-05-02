using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.IntegrationEvents.Events;

namespace Basket.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasketCheckout, BasketCheckoutEvent>();
    }
}