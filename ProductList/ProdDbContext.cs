using Microsoft.EntityFrameworkCore;

public class ProdDbContext:DbContext
{
    public ProdDbContext(DbContextOptions<ProdDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = 1,
                Name = "Lunch Pack",
                Description = "part of the back to school package"
               
            },
            new Product
            {
                ProductId = 2,
                Name = "Books",
                Description = "part of the back to school package"
               
            },
            new Product
            {
                ProductId = 3,
                Name = "Shoes",
                Description = "part of the back to school package"
                
            });

       
    }
}