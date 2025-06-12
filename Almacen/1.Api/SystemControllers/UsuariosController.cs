using Api._1.API.Dto;
using Api._1.API.Utils;
using Api._2.Common.Models.System;
using Api._3.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api._1.API.SystemControllers
{
    [Route("api/v1/hades/usuarios")]
    public class UsuariosController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<string>>> GetUsers()
        {
            var users = await (new ApiService<List<DatosPersonalesDto>>(ServicesConstants.HadesBaseUri).GetAsync(ServicesConstants.HadesGetPersonalMethod, "eea1272d9bdec13a433eaeeb16fca2335dd3b438"));
            return Ok(ApiUtils.Response(users));
        }
    }
}
