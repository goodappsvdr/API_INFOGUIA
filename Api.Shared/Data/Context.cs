using System;
using System.Collections.Generic;
using Api.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Shared.Data;

public partial class Context : DbContext
{
    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public virtual DbSet<BranchsOffice> BranchsOffices { get; set; }

    public virtual DbSet<BranchsUser> BranchsUsers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }



    public virtual DbSet<Listing> Listings { get; set; }

    public virtual DbSet<ListingDirectory> ListingDirectories { get; set; }

    public virtual DbSet<ListingHour> ListingHours { get; set; }

    public virtual DbSet<ListingImage> ListingImages { get; set; }

    public virtual DbSet<ListingPaymentMethod> ListingPaymentMethods { get; set; }

    public virtual DbSet<ListingPhone> ListingPhones { get; set; }

    public virtual DbSet<ListingService> ListingServices { get; set; }

    public virtual DbSet<ListingSocialLink> ListingSocialLinks { get; set; }

    public virtual DbSet<ListingTag> ListingTags { get; set; }

    public virtual DbSet<ListingUser> ListingUsers { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BranchsOffice>(entity =>
        {
            entity.HasKey(e => e.BranchOfficeId);

            entity.Property(e => e.BranchOfficeId).HasColumnName("BranchOfficeID");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SalesPoint)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BranchsUser>(entity =>
        {
            entity.HasKey(e => e.BranchUserId);

            entity.Property(e => e.BranchUserId).HasColumnName("BranchUserID");
            entity.Property(e => e.BranchOfficeId).HasColumnName("BranchOfficeID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B99971774");

            entity.HasIndex(e => e.CreatedByUserId, "IX_Categories_CreatedByUserID");

            entity.HasIndex(e => e.ModifiedByUserId, "IX_Categories_ModifiedByUserID");

            entity.HasIndex(e => e.ParentCategoryId, "IX_Categories_ParentCategoryID");

            entity.HasIndex(e => e.TenantId, "IX_Categories_TenantID");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ParentCategoryId).HasColumnName("ParentCategoryID");
            entity.Property(e => e.TenantId).HasColumnName("TenantID");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__F2D21A96FAB7A431");

            entity.HasIndex(e => e.ProvinceId, "IX_Cities_ProvinceID");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");
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
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.LogoUrl).HasMaxLength(512);
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
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
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
        });

        modelBuilder.Entity<ListingHour>(entity =>
        {
            entity.HasKey(e => e.ListingHourId).HasName("PK__ListingH__592C613EFBDC1CE4");

            entity.HasIndex(e => e.ListingId, "IX_ListingHours_ListingID");

            entity.Property(e => e.ListingHourId).HasColumnName("ListingHourID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
        });

        modelBuilder.Entity<ListingImage>(entity =>
        {
            entity.HasKey(e => e.ListingImageId).HasName("PK__ListingI__C6DC7267363AC772");

            entity.HasIndex(e => e.ListingId, "IX_ListingImages_ListingID");

            entity.Property(e => e.ListingImageId).HasColumnName("ListingImageID");
            entity.Property(e => e.Caption).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.ImageUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
        });

        modelBuilder.Entity<ListingPaymentMethod>(entity =>
        {
            entity.HasKey(e => new { e.ListingId, e.PaymentMethodId }).HasName("PK__ListingP__92FDA2EF060DA934");

            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
        });

        modelBuilder.Entity<ListingPhone>(entity =>
        {
            entity.HasKey(e => e.ListingPhoneId).HasName("PK__ListingP__173A7A4BF4234577");

            entity.HasIndex(e => e.ListingId, "IX_ListingPhones_ListingID");

            entity.Property(e => e.ListingPhoneId).HasColumnName("ListingPhoneID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.PhoneType).HasMaxLength(50);
        });

        modelBuilder.Entity<ListingService>(entity =>
        {
            entity.HasKey(e => new { e.ListingId, e.ServiceId }).HasName("PK__ListingS__036F05FE36A3A0AF");

            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
        });

        modelBuilder.Entity<ListingSocialLink>(entity =>
        {
            entity.HasKey(e => e.ListingSocialLinkId).HasName("PK__ListingS__F35A644047C0C02F");

            entity.HasIndex(e => e.ListingId, "IX_ListingSocialLinks_ListingID");

            entity.Property(e => e.ListingSocialLinkId).HasColumnName("ListingSocialLinkID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.NetworkName).HasMaxLength(50);
            entity.Property(e => e.ProfileUrl).HasMaxLength(512);
        });

        modelBuilder.Entity<ListingTag>(entity =>
        {
            entity.HasKey(e => new { e.ListingId, e.TagId }).HasName("PK__ListingT__69697154B3470137");

            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
        });

        modelBuilder.Entity<ListingUser>(entity =>
        {
            entity.Property(e => e.ListingUserId).HasColumnName("ListingUserID");
            entity.Property(e => e.ListingId).HasColumnName("ListingID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK__PaymentM__DC31C1F3A5B8D7A3");

            entity.HasIndex(e => e.TenantId, "IX_PaymentMethods_TenantID");

            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TenantId).HasColumnName("TenantID");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.ProvinceId).HasName("PK__Province__FD0A6FA393A5CE47");

            entity.HasIndex(e => e.CountryId, "IX_Provinces_CountryID");

            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB0EAA74C382E");

            entity.HasIndex(e => e.TenantId, "IX_Services_TenantID");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IconUrl).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TenantId).HasColumnName("TenantID");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.Property(e => e.StateId).HasColumnName("StateID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.State1).HasColumnName("State");
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

            entity.HasIndex(e => e.CityId, "IX_Tenants_CityID");

            entity.HasIndex(e => e.CreatedByUserId, "IX_Tenants_CreatedByUserID");

            entity.HasIndex(e => e.ModifiedByUserId, "IX_Tenants_ModifiedByUserID");

            entity.Property(e => e.TenantId).HasColumnName("TenantID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedByUserId).HasColumnName("ModifiedByUserID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC2B1B6163");

            entity.HasIndex(e => e.CreatedByUserId, "IX_Users_CreatedByUserID");

            entity.HasIndex(e => e.ModifiedByUserId, "IX_Users_ModifiedByUserID");

            entity.HasIndex(e => e.RoleId, "IX_Users_RoleID");

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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
