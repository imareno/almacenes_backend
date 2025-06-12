using Microsoft.EntityFrameworkCore;

namespace Almacen._4.DataBase.DataBaseContext
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
        {
        }
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }
    }
}
