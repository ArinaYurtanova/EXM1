using ClassLibrary.Models;
using System.Data.Entity;


namespace ClassLibrary
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DataBase")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Bluda> Bludas { get; set; }
        public virtual DbSet<Produckt> Products { get; set; }

    }
}
