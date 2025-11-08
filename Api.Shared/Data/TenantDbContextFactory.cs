using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.Data
{
    public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
    {
        public TenantDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();

            // Connection string para diseño (migraciones)
            optionsBuilder.UseSqlServer(
                 "Server=147.93.176.131;Database=InfoGuia_TenantStore;User Id=InfoGuia_TenantStore;Password=InfoGuia_TenantStore*963;TrustServerCertificate=True;MultipleActiveResultSets=true"
            );

            return new TenantDbContext(optionsBuilder.Options);
        }
    }
}
