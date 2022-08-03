using ElsaTestDBDemo.DomainDatabase.Entites;
using Microsoft.EntityFrameworkCore;

namespace ElsaTestDBDemo.DomainDatabase
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Request> Requests { get; set; }
    }
}
