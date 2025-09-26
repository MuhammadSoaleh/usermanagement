using Microsoft.EntityFrameworkCore;
using usermanagement.Models;

namespace usermanagement.Data
{
    public class Applicationdbcontext : DbContext

    {
        private readonly DbContextOptions options;

        public Applicationdbcontext(DbContextOptions options) : base(options)
        {
            this.options = options;
        }

        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<company> Companies { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Employee> employees { get; set; }
    }
}
