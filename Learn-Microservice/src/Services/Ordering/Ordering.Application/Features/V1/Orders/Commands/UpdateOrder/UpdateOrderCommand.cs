using AutoMapper;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders.Common;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using Infrastructure.Mappings;


namespace Ordering.Application.Features.V1.Orders.Commands.UpdateOrder;

public class UpdateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<OrderDto>>, IMapFrom<Order>
{
    public Guid Id { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateOrderCommand, Order>()
            .ForMember(dest => dest.Status, 
                opts => opts.Ignore());
    }

    public void SetId(Guid id) => Id = id;
}

