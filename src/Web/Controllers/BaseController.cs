using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    [ApiController]
    [Authorize]
    [Logger]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class BaseController {}
}