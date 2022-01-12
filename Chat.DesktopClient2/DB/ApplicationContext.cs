using Microsoft.EntityFrameworkCore;

namespace Chat.DesktopClient2.DB
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
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloappdb4;Trusted_Connection=True;");
        }
    }
}
