
using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Data;
public class ApplicationDbContext :IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories {  get; set; }
	public DbSet<CoverType> CoverTypes { get; set; }
    public DbSet<Product> Products { get; set; }
	public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
	public DbSet<OrderHeader> OrderHeaders { get; set; }
	public DbSet<OrderDetail> OrderDetail { get; set; }
    public DbSet<Comment> Comments { get; set; }

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //	base.OnModelCreating(builder);
    //	// Customize the ASP.NET Identity model and override the defaults if needed.
    //	// For example, you can rename the ASP.NET Identity table names and more.
    //	// Add your customizations after calling base.OnModelCreating(builder);
    //}
}
