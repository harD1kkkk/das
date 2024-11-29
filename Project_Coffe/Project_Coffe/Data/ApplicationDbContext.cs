using Project_Coffe.Entities;
using Microsoft.EntityFrameworkCore;

namespace Project_Coffe.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)                
                .WithMany(u => u.Orders)             
                .HasForeignKey(o => o.UserId)        
                .OnDelete(DeleteBehavior.Cascade);   

       
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)              
                .IsUnique();                         

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");     

            base.OnModelCreating(modelBuilder);
        }
    }
}
