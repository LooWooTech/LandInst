using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using MySql.Data.Entity;

namespace Loowoo.LandInst.Manager
{
    public class DBDataContext : DbContext
    {
        public DBDataContext()
            : base("name=DBDataContext")
        {
        }

        public DBDataContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Member> Members { get; set; }
    }
}
