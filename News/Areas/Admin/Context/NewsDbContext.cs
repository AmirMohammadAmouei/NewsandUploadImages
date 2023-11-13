using Microsoft.EntityFrameworkCore;
using News.Areas.Admin.Models;
using News.Models;

namespace News.Context
{
    public class NewsDbContext : DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
        {
        }

        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<NewsModel> News { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AftabSaNewsDb;Integrated Security=true";
                optionsBuilder.UseSqlServer(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsModel>().HasKey(x => x.Id);
            modelBuilder.Entity<NewsModel>().HasOne(x =>
            x.Category).WithMany(x => x.News).HasForeignKey(x => x.CategoryId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
