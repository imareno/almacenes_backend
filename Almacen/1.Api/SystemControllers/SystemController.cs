using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api._1.API.SystemControllers
{
    public class SystemController : Controller
    {

        [HttpGet("api/v1/system/status")]
        [AllowAnonymous]
        public ActionResult Status()
        {
            return Ok("correcto viaticos");
        }
    }
}
