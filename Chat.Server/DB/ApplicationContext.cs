using Microsoft.EntityFrameworkCore;

namespace Chat.DB
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Data> Messeges { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=serverHello;Trusted_Connection=True;");
        }
    }
}
