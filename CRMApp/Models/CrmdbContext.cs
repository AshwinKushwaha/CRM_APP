using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CRMApp.Models;

public partial class CrmdbContext : DbContext
{
    public CrmdbContext()
    {
    }

    public CrmdbContext(DbContextOptions<CrmdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
