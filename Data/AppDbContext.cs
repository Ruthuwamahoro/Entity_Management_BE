using Microsoft.EntityFrameworkCore;
using EntityApi.Models;

namespace EntityApi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        public DbSet<Entity> Entities { get; set; }
    }
}

