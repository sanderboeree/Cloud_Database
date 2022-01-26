using OnlineStore.Api.Infrastructure.ExceptionHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace OnlineStore.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(void), 403)]
    [ProducesResponseType(typeof(ApiError), 500)]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public virtual ObjectResult Forbid([ActionResultObjectValue] object value)
        {
            return StatusCode(403, value);
        }
    }
}
