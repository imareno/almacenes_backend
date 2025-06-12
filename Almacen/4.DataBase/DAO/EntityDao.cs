using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using Almacen._2.Common.Models.System;
using Almacen._4.DataBase;
using Almacen._4.DataBase.DataBaseContext;
using Api._2.Common.Models.Generic;
using Api._1.API.Utils;
using Constants = Almacen._2.Common.Models.System.Constants;


namespace Api._4.DataBase
{
    public class EntityDao<T> : Methods<T> where T : Entity
    {
        private readonly DataBaseContext _dbContext;
        public EntityDao(DataBaseContext dbContext)
        {
            this._dbContext = dbContext;
        }
        protected IQueryable<T> GetEntities(bool joined = true)
        {
            var data = _dbContext.Set<T>().Where(x => x.FechaEliminacion == null).OrderByDescending(x => x.FechaCreacion);
            return joined ? GetJoindedData(data) : data;
        }
        public IQueryable<T> GetJoindedData(IQueryable<T> data)
        {
            return GetPropertyInfo().Aggregate(data, (x, propertyInfo) => x.Include(propertyInfo.Name));
        }
        //GET METHODS
        public IQueryable<T> GetAllEntities(bool joined = true)
        {
            return GetEntities(joined);
        }
        public T GetEntityById(Guid id, bool joined = true)
        {
            return (GetEntities().Any(o => o.Id == id) ? GetEntities(joined).FirstOrDefault(o => o.Id == id) : throw new AppNotFoundException(Constants.RecursoInexistente))!;
        }
        public T GetFilteredEntity(string filterStatement, bool joined = true)
        {
            return GetEntities(joined).Where(filterStatement).FirstOrDefault() ?? throw new AppNotFoundException(Constants.RecursoInexistente);
            //PROBAR SI DA NULO
        }
        public IQueryable<T> GetCollection(CollectionParameters parameters, bool joined = true)
        {
            var sorting = string.IsNullOrEmpty(parameters.Sort) ? $"{nameof(Entity.FechaCreacion)} desc" : $"{JsonDocument.Parse(parameters.Sort).RootElement.GetProperty("selector").GetString()} {(JsonDocument.Parse(parameters.Sort).RootElement.GetProperty("desc").GetBoolean() == true ? "desc" : "asc")}";
            var result = string.IsNullOrEmpty(parameters.Filter) ? GetEntities(joined) : GetEntities(joined).Where(parameters.Filter);
            result = parameters.Take == 0 ? result : result.Skip((parameters.Skip - 1) * parameters.Take).Take(parameters.Take);
            return result.OrderBy(sorting);
        }

        //CUD METHODS
        public T Create(T entity, bool joined = true)
        {
            entity.FechaCreacion = DateTime.Now;
            var createdId = _dbContext.Set<T>().Add(entity).Entity.Id;
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var result = ex.Message;
                throw;
            }
            return GetEntityById(createdId, joined);
        }
        public T Update(T entity, Guid id, bool joined = true)
        {
            if (entity.Id != id)
                throw new AppBadRequestException(Constants.LlavePrimariaDiferente);
            var toUpdate = GetEntityById(id);
            _dbContext.Entry(toUpdate).CurrentValues.SetValues(entity);
            _dbContext.Entry(toUpdate).Property(x => x.Id).IsModified = false;
            _dbContext.Entry(toUpdate).Property(x => x.UsuarioCreacion).IsModified = false;
            _dbContext.Entry(toUpdate).Property(x => x.FechaCreacion).IsModified = false;
            _dbContext.Entry(toUpdate).Property(x => x.Token).IsModified = false;
            _dbContext.SaveChanges();
            return GetEntityById(entity.Id, joined);
        }

        public void Delete(Guid id)
        {
            if (!GetEntities().Any(o => o.Id == id))
            {
                throw new AppNotFoundException(Constants.RecursoInexistente);
            }
            _dbContext.Set<T>().Single(o => o.Id == id).FechaEliminacion = DateTime.UtcNow;
            _dbContext.SaveChanges();
        }


    }
}
