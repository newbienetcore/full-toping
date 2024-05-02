using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.Application.Common.Models.PaginatedList;
using Notifications.Application.NotificationGroups.Commands.Create;
using Notifications.Application.NotificationGroups.Commands.Delete;
using Notifications.Application.NotificationGroups.Commands.Update;
using Notifications.Application.NotificationGroups.Models;
using Notifications.Application.NotificationGroups.Queries.Get;
using Notifications.Application.NotificationGroups.Queries.GetsWithPagination;

namespace Notifications.WebApi.Controllers;

public class NotificationGroupController : ApiControllerBase
{
    [HttpGet]
    [Route("api/notification-groups")]
    public async Task<IActionResult> GetAllAsync([FromQuery]GetNotificationGroupsWithWithPaginationQuery query)
    {
        return Ok(await Mediator.Send(query).ConfigureAwait(false));
    }
    
    [HttpGet]
    [Route("api/notification-groups/{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")]Guid notificationGroupId)
    {
        return Ok(await Mediator.Send(new GetNotificationGroupQuery(notificationGroupId)));
    }

    [HttpPost]
    [Route("api/notification-groups")]
    public async Task<IActionResult> CreateAsync([FromBody]CreateNotificationGroupCommand command)
    {
        return Ok(await Mediator.Send(command).ConfigureAwait(false));
    }
    
    [HttpPut]
    [Route("api/notification-groups/{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute(Name = "id")]Guid notificationGroupId, [FromBody]UpdateNotificationGroupCommand command)
    {
        return Ok(await Mediator.Send(command).ConfigureAwait(false));
    }
    
    [HttpDelete]
    [Route("api/notification-groups/{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] Guid notificationGroupId)
    {
        return Ok(await Mediator.Send(new DeleteNotificationGroupCommand(notificationGroupId)));
    }

}