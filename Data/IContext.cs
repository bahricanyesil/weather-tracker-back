using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data
{
    public class IContext : DbContext
    {
        public IContext(DbContextOptions<IContext> opt) : base(opt)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Participance> Participances { get; set; }
        public DbSet<Friend> Friends { get; set; }
    }
}