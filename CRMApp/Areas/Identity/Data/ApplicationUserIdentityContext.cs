using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace CRMApp.Areas.Identity.Data;

public class ApplicationUserIdentityContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationUserIdentityContext(DbContextOptions<ApplicationUserIdentityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerContact> CustomerContacts { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    public DbSet<Note> Notes { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        builder.Entity<Customer>().ToTable("Customers");
        builder.Entity<CustomerContact>().ToTable("CustomerContacts");
        builder.Entity<CustomerContact>()
            .HasOne(p => p.Customer)
            .WithMany(c => c.Contacts)
            .HasForeignKey(p => p.CustomerId);

        builder.Entity<ActivityLog>().ToTable("ActivityLogs");
        builder.Entity<Note>().ToTable("Notes");

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.UserName).HasMaxLength(255);
    }
}