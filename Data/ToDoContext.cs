using Microsoft.EntityFrameworkCore;
using Dev_Groundup.Models;

namespace Dev_Groundup.Data
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
        }

        public DbSet<ToDoItem>? ToDoItem { get; set; }
    }
}
