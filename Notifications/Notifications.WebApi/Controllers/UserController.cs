using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.Application.Users.Commands.SignIn;
using Notifications.Application.Users.Commands.SignUp;
using Notifications.Application.Users.Models;

namespace Notifications.WebApi.Controllers;

public class UserController : ApiControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [Route("api/users/sign-up")]
    public async Task<ActionResult<AuthenticationResult>> SignUpAsync([FromBody]SignUpCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPost]
    [AllowAnonymous]
    [Route("api/users/sign-in")]
    public async Task<ActionResult<AuthenticationResult>> SignInAsync([FromBody]SignInCommand command)
    {
        return await Mediator.Send(command);
    }

}