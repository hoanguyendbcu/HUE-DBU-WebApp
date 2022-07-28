using DBCU_WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class DBCU_WebContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Gazetteer> Gazetteer { set; get; }
    public DbSet<ActivityMA> ActivityMA { set; get; }
    public DbSet<Organization> Organization { set; get; }
    public DbSet<OperationPlans> OperationPlans { set; get; }
    public DbSet<OperationPlanWeek> OperationPlanWeek { set; get; }

    public DbSet<Category> Categories { set; get; }
    public DbSet<News> News { set; get; }
    public DbSet<NewsCategory> NewsCategorys { set; get; }


    public DBCU_WebContext(DbContextOptions<DBCU_WebContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        //builder.Entity<PostCategory>().HasKey(p => new { p.PostID, p.CategoryID });
        // Tạo Index cho cột Slug bảng Category
        builder.Entity<Category>(entity =>
        {
            entity.HasIndex(p => p.Slug);
        });

        // Tạo key của bảng là sự kết hợp PostID, CategoryID, qua đó
        // tạo quan hệ many to many giữa Post và Category
        builder.Entity<NewsCategory>().HasKey(p => new { p.NewsID, p.CategoryID });

    }
}
