using Microsoft.EntityFrameworkCore;
using Dev_Groundup.Models;

namespace Dev_Groundup.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User>? Users { get; set; }
    }
}
