using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DotNetAppSqlDb.Models
{
    public class MyDatabaseContext : DbContext
    {
        public MyDatabaseContext() : base("name=Pivovar")
        {
        }

        public DbSet<Temperature> Temperatures { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<IoTDevice> Devices { get; set; }
    }
}
