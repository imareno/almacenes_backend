
using System.Collections;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Api._2.Common.Models.Generic;

namespace Almacen._4.DataBase
{
    public class Methods<T> where T : Entity
    {
        protected static IEnumerable<PropertyInfo> GetPropertyInfo()
        {
            return (typeof(T).GetProperties().Where(x => !x.GetAccessors()[0].IsFinal && x.GetAccessors()[0].IsVirtual)
                .Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))).Concat(typeof(T).GetProperties()
                .Where(x => !x.GetAccessors()[0].IsFinal && x.GetAccessors()[0].IsVirtual)
                .Where(x => x.GetCustomAttributes(true).Any(o => o is ForeignKeyAttribute)));
        }
        protected IQueryable<T> GetJoindedData(IQueryable<T> data)
        {
            return GetPropertyInfo().Aggregate(data, (x, propertyInfo) => x.Include(propertyInfo.Name));
        }
    }
}
