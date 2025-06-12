using Almacen._4.DataBase.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Almacen._1.Api.Extensions
{
    public static class DbContextExtension
    {
        public static void ConfigureDbContext(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<DataBaseContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DbConnection")));
        }
    }
}
