using Api.Shared.DTOs.Tenant;
using Api.Shared.Models;
using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;

namespace Api.Shared.Data
{
    /// <summary>
    /// DbContext para gestionar la lista de Tenants
    /// Se conecta a la base de datos InfoGuia_TenantStore
    /// </summary>
    public class TenantDbContext : EFCoreStoreDbContext<TenantInfo>
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TenantInfo>(entity =>
            {
                // Configurar propiedades adicionales
                entity.Property(e => e.DatabaseName)
                    .HasMaxLength(100);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}