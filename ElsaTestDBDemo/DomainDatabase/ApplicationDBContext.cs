using Microsoft.EntityFrameworkCore;

namespace ElsaTestDBDemo.DomainDatabase
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        { }
    }
}
