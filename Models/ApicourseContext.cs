using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APICourse.Models;

public partial class ApicourseContext : DbContext
{
    public ApicourseContext()
    {
    }

    public ApicourseContext(DbContextOptions<ApicourseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=APICourse;User Id=sa;Password=Thanglq123;Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.PId).HasName("PK__Product__DD36D5027282F7CF");

            entity.ToTable("Product");

            entity.Property(e => e.PId).HasColumnName("pID");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateAdded");
            entity.Property(e => e.ProductCategory)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("productCategory");
            entity.Property(e => e.ProductDescription)
                .HasColumnType("text")
                .HasColumnName("productDescription");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("productName");
            entity.Property(e => e.ProductPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("productPrice");
            entity.Property(e => e.StockQuantity)
                .HasDefaultValue(0)
                .HasColumnName("stockQuantity");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
