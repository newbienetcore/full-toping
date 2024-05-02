using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers.VersionOne;

[ApiController]
[Route("api/v1/microservices/[controller]")]
public class BaseController : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}