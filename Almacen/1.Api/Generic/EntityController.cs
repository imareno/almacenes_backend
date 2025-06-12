using Api._1.API.Utils;
using Api._2.Common.Models.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Almacen._2.Common.Models.System;
using Api._4.DataBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Api._1.API.Generic
{
    //[Authorize]
    [ApiController]
    public abstract class EntityController<T, Dto> : ControllerBase where T : Entity
    {
        private readonly EntityDao<T> _entityDao;
        private readonly IMapper _mapper;

        protected EntityController(EntityDao<T> entityDao, IMapper mapper)
        {
            this._entityDao = entityDao;
            this._mapper = mapper;
        }

        [HttpGet("{id:Guid}/{joined:bool=true}")]
        public ActionResult<ApiResponse<Dto>> GetById(Guid id, Boolean joined)
        {
            return ReturnSingleEntity_Act(GetById_Dao(id));
        }

        //[HttpGet("{joined:bool=true}")]
        //public ActionResult<ApiResponse<IEnumerable<Dto>>> GetAll(Boolean joined)
        //{
        //    return ReturnEntityCollection_Act(GetAll_Dao(joined));
        //}

        [HttpGet("{joined:bool=true}")]
        public ActionResult<ApiResponse<IEnumerable<Dto>>> GetCollection([FromQuery] CollectionParameters parameters, Boolean joined)
        {
            return ReturnEntityCollection_Act(GetCollection_Dao(parameters, joined));
        }

        [HttpPost("{joined:bool=true}")]
        public ActionResult<ApiResponse<Dto>> CreateT([FromBody] T entity, Boolean joined)
        {
            return CreatedEntity_Act(Create_Dao(InsertUserClaim(entity), joined));
        }

        [HttpPut("{id}/{joined:bool=true}")]
        public ActionResult UpdateT([FromBody] T entity, Guid id, Boolean joined)
        {
            Update_Dao(entity, id, joined);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteT(Guid id)
        {
            Delete_Dao(id);
            return NoContent();
        }

        /////////////////////////////////////////////////////////////////////RESPONSE METHODS///////////////////////////////////////////////////////////////
        protected ActionResult<ApiResponse<Dto>> ReturnSingleEntity_Act(T entity)
        {
            return Ok(ApiUtils.Response(_mapper.Map<Dto>(entity)));
        }
        protected ActionResult<ApiResponse<IEnumerable<Dto>>> ReturnEntityCollection_Act(IEnumerable<T> entities)
        {
            return Ok(ApiUtils.Response(_mapper.Map<IEnumerable<Dto>>(entities)));
        }
        protected ActionResult<ApiResponse<Dto>> CreatedEntity_Act(T entity)
        {
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, ApiUtils.Response(this._mapper.Map<Dto>(entity)));
        }
        /////////////////////////////////////////////////////////////////////DAO METHODS///////////////////////////////////////////////////////////////////
        protected T GetById_Dao(Guid id)
        {
            return this._entityDao.GetEntityById(id);
        }
        protected IEnumerable<T> GetAll_Dao(bool joined = true)
        {
            return this._entityDao.GetAllEntities(joined);
        }
        protected IEnumerable<T> GetCollection_Dao(CollectionParameters parameters, bool joined = true)
        {
            return this._entityDao.GetCollection(parameters, joined);
        }
        protected T Create_Dao(T entity, bool joined = true)
        {
            return this._entityDao.Create(InsertUserClaim(entity), joined);
        }

        protected T Update_Dao(T entity, Guid id, bool joined = true)
        {
            return this._entityDao.Update(entity, id, joined);
        }

        protected void Delete_Dao(Guid id)
        {
            this._entityDao.Delete(id);
        }
        /////////////////////////////////////////////////////////////////////CONTROLLER METHODS///////////////////////////////////////////////////////////////////
        protected T InsertUserClaim(T entity)
        {
            //var claimkey = GetIdClaim("jti");
            if (string.IsNullOrEmpty(Request.Headers["Authorization"].ToString())) return entity;

            var authHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var token = (JwtSecurityToken)new JwtSecurityTokenHandler().ReadToken(authHeader);
            entity.UsuarioCreacion = int.Parse(token.Claims.First(claim => claim.Type == "idUser").Value);
            entity.Token = Guid.Parse(token.Claims.First(claim => claim.Type == "idToken").Value);
            
            return entity;
        }
        protected Guid GetIdClaim(string claimKey)
        {
            var authHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadToken(authHeader) as JwtSecurityToken;
            return Guid.Parse(token.Claims.First(claim => claim.Type == claimKey).Value);
        }

        protected string GetStringValueClaim(string claimKey)
        {
            var authHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadToken(authHeader) as JwtSecurityToken;
            return token.Claims.First(claim => claim.Type == claimKey).Value;
        }
    }
}