using Api.Shared.Interface;
using Api.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Api.Shared.Data;

public partial class Context : DbContext
{



    public Context(DbContextOptions<Context> options) : base(options)
    {
    }


    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Models.Directory> Directories { get; set; }

    public virtual DbSet<Listing> Listings { get; set; }

    public virtual DbSet<ListingDirectory> ListingDirectories { get; set; }

    public virtual DbSet<ListingHour> ListingHours { get; set; }

    public virtual DbSet<ListingImage> ListingImages { get; set; }

    public virtual DbSet<ListingPaymentMethod> ListingPaymentMethods { get; set; }

    public virtual DbSet<ListingPhone> ListingPhones { get; set; }

    public virtual DbSet<ListingService> ListingServices { get; set; }

    public virtual DbSet<ListingSocialLink> ListingSocialLinks { get; set; }

    public virtual DbSet<ListingTag> ListingTags { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //    ApplyAuditInfo();
    //    return base.SaveChangesAsync(cancellationToken);
    //}

    //private void ApplyAuditInfo()
    //{
    //    var entries = ChangeTracker.Entries<IAuditableEntity>();

    //    foreach (var entry in entries)
    //    {
    //        if (entry.State == EntityState.Added)
    //        {
    //            entry.Entity.CreatedAt = DateTime.UtcNow;
    //            entry.Entity.CreatedByUserID = _currentUserId;
    //        }
    //        else if (entry.State == EntityState.Modified)
    //        {
    //            entry.Entity.ModifiedAt = DateTime.UtcNow;
    //            entry.Entity.ModifiedByUserID = _currentUserId;
    //        }
    //    }
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B99971774");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ParentCategoryId).HasColumnName("ParentCategoryID");
            entity.Property(e => e.TenantId).HasColumnName("TenantID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.CategoryCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_Categories_CreatedByUser");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.CategoryModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .HasConstraintName("FK_Categories_ModifiedByUser");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK_Categories_Self_Parent");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Categories)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Categories_Tenants");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__F2D21A96FAB7A431");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");

            entity.HasOne(d => d.Province).WithMany(p => p.Cities)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cities_Provinces");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__Countrie__10D160BFA6A10B3C");

            entity.HasIndex(e => e.Name, "UQ__Countrie__737584F6038A11C2").IsUnique();

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Models.Directory>(entity =>
        {
            entity.HasKey(e => e.DirectoryId).HasName("PK__Director__3D93EF02310A129E");

            entity.HasIndex(e => e.TenantId, "IX_Directories_TenantID");

            entity.Property(e => e.DirectoryId).HasColumnName("DirectoryID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TenantId).HasColumnName("TenantID");
        });

        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(e => e.ListingId).HasName("PK__Listings__BF3EBEF015E53387");

            entity.HasIndex(e => e.CategoryId, "IX_Listings_CategoryID");

            entity.HasIndex(e => e.TenantId, "IX_Listings_TenantID");

            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CatalogUrl).HasMaxLength(512);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.LogoUrl).HasMaxLength(512);
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ShortDescription).HasMaxLength(500);
            entity.Property(e => e.TenantId).HasColumnName("TenantID");
            entity.Property(e => e.VideoUrl).HasMaxLength(512);
            entity.Property(e => e.WebsiteUrl).HasMaxLength(512);
        });

        modelBuilder.Entity<ListingDirectory>(entity =>
        {
            entity.HasKey(e => new { e.ListingId, e.DirectoryId }).HasName("PK__ListingD__8CE7800067021207");

            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.DirectoryId).HasColumnName("DirectoryID");
        });

        modelBuilder.Entity<ListingHour>(entity =>
        {
            entity.HasKey(e => e.ListingHourId).HasName("PK__ListingH__592C613EFBDC1CE4");

            entity.HasIndex(e => e.ListingId, "IX_ListingHours_ListingID");

            entity.Property(e => e.ListingHourId).HasColumnName("ListingHourID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ListingId).HasColumnName("ListingID");
        });

        modelBuilder.Entity<ListingImage>(entity =>
        {
            entity.HasKey(e => e.ListingImageId).HasName("PK__ListingI__C6DC7267363AC772");

            entity.HasIndex(e => e.ListingId, "IX_ListingImages_ListingID");

            entity.Property(e => e.ListingImageId).HasColumnName("ListingImageID");
            entity.Property(e => e.Caption).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ImageUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ListingId).HasColumnName("ListingID");
        });

        modelBuilder.Entity<ListingPaymentMethod>(entity =>
        {
            entity.HasKey(e => new { e.ListingId, e.PaymentMethodId }).HasName("PK__ListingP__92FDA2EF060DA934");

            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");
        });

        modelBuilder.Entity<ListingPhone>(entity =>
        {
            entity.HasKey(e => e.ListingPhoneId).HasName("PK__ListingP__173A7A4BF4234577");

            entity.HasIndex(e => e.ListingId, "IX_ListingPhones_ListingID");

            entity.Property(e => e.ListingPhoneId).HasColumnName("ListingPhoneID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.PhoneType).HasMaxLength(50);
        });

        modelBuilder.Entity<ListingService>(entity =>
        {
            entity.HasKey(e => new { e.ListingId, e.ServiceId }).HasName("PK__ListingS__036F05FE36A3A0AF");

            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
        });

        modelBuilder.Entity<ListingSocialLink>(entity =>
        {
            entity.HasKey(e => e.ListingSocialLinkId).HasName("PK__ListingS__F35A644047C0C02F");

            entity.HasIndex(e => e.ListingId, "IX_ListingSocialLinks_ListingID");

            entity.Property(e => e.ListingSocialLinkId).HasColumnName("ListingSocialLinkID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.NetworkName).HasMaxLength(50);
            entity.Property(e => e.ProfileUrl).HasMaxLength(512);
        });

        modelBuilder.Entity<ListingTag>(entity =>
        {
            entity.HasKey(e => new { e.ListingId, e.TagId }).HasName("PK__ListingT__69697154B3470137");

            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.TagId).HasColumnName("TagID");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK__PaymentM__DC31C1F3A5B8D7A3");

            entity.HasIndex(e => e.TenantId, "IX_PaymentMethods_TenantID");

            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TenantId).HasColumnName("TenantID");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.ProvinceId).HasName("PK__Province__FD0A6FA393A5CE47");

            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Country).WithMany(p => p.Provinces)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Provinces_Countries");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A5E3654EA");

            entity.HasIndex(e => e.Name, "UQ__Roles__737584F6B9F64D24").IsUnique();

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB0EAA74C382E");

            entity.HasIndex(e => e.TenantId, "IX_Services_TenantID");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TenantId).HasColumnName("TenantID");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__Tags__657CFA4C3DE630B2");

            entity.HasIndex(e => e.TenantId, "IX_Tags_TenantID");

            entity.HasIndex(e => e.Name, "UQ__Tags__737584F6F04CCD75").IsUnique();

            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TenantId).HasColumnName("TenantID");
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.TenantId).HasName("PK__Tenants__2E9B47813336E98C");

            entity.Property(e => e.TenantId).HasColumnName("TenantID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.City).WithMany(p => p.Tenants)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tenants_Cities");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.TenantCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_Tenants_CreatedByUser");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.TenantModifiedByUsers)
                .HasForeignKey(d => d.ModifiedByUserId)
                .HasConstraintName("FK_Tenants_ModifiedByUser");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC2B1B6163");

            entity.HasIndex(e => e.TenantId, "IX_Users_TenantID").HasFilter("([TenantID] IS NOT NULL)");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053443B3B561").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.ImgProfile).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.TenantId).HasColumnName("TenantID");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.InverseCreatedByUser)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_Users_CreatedByUser");

            entity.HasOne(d => d.ModifiedByUser).WithMany(p => p.InverseModifiedByUser)
                .HasForeignKey(d => d.ModifiedByUserId)
                .HasConstraintName("FK_Users_ModifiedByUser");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Users)
                .HasForeignKey(d => d.TenantId)
                .HasConstraintName("FK_Users_Tenants");
        });

        // NOTA: NO necesitamos filtros globales porque cada tenant tiene su propia base de datos
        // Finbuckle automáticamente conecta a la base de datos correcta según el tenant

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}