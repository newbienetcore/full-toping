using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers.VersionOne;

[ApiController]
[Route("api/v1/open-school-microservices/[controller]")]
public class BaseController : ControllerBase
{
}