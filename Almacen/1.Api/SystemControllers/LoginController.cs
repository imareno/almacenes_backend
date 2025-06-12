using Api._1.API.Utils;
using Api._3.Services;
using Api._1.API.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Security.Claims;
using Almacen._1.Api.DTO;
using Almacen._2.Common.Models.System;
using Api._1.API.Dto;
using Api._2.Common.Models.Security;
using Api._2.Common.Models.System;
using Api._4.DataBase;
using AutoMapper;

namespace Api._1.API.SystemControllers
{

    [Route("api/v1/Login")]
    public class LoginController : EntityController<Login, LoginDto>
    {
        public LoginController(EntityDao<Login> entityDao, IMapper mapper) : base(entityDao, mapper)
        {
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] UserLoginDto userLoginDto)
        {
            var idToken = Guid.NewGuid();
            var response = await (new ApiService<TokenDto>(ServicesConstants.HadesBaseUri)).PostAsync<UserLoginDto, TokenDto>(ServicesConstants.HadesTokenMethod, userLoginDto);
            var user = await (new ApiService<UserDto>(ServicesConstants.HadesBaseUri).GetAsync(ServicesConstants.HadesGetUserMethod, response.token));
            var token = JwtHelper.GenerateJwt(user.persona_id, 60, new Claim("idUser", JsonSerializer.Serialize(user.id)), new Claim("idToken", idToken.ToString()), new Claim("tkn", response.token));

            var login = new Login()
            {
                Id = idToken,
                UsuarioCreacion = user.id,
                Ip = HttpContext.Connection.RemoteIpAddress!.ToString(),
                TokenJwt = token
            };
            var newLogin = Create_Dao(login);

            return Ok(ApiUtils.Response(newLogin.TokenJwt));
        }

        [HttpGet("UserData")]
        public async Task<ActionResult<ApiResponse<UserDto>>> getUserData ()
        {
            var user = await(new ApiService<UserDto>(ServicesConstants.HadesBaseUri).GetAsync(ServicesConstants.HadesGetUserMethod, GetStringValueClaim("tkn")));
            return Ok(ApiUtils.Response(user));
        }

        [HttpPost("refresh")]
        public ActionResult<ApiResponse<string>> RefreshToken()
        {
            var currentToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var response = JwtHelper.RefreshAccessToken(currentToken, 60);
            return Ok(ApiUtils.Response(response));
        }
    }
}
