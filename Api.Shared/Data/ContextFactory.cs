using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Shared.Data
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();

            // Connection string temporal para diseño (migraciones)
            optionsBuilder.UseSqlServer(
          "Server=147.93.176.131;Database=INFOGUIA;User Id=INFOGUIA;Password=INFOGUIA*963;TrustServerCertificate=True;MultipleActiveResultSets=true"
      );
            // Crear el Context sin dependencias para migraciones
            return new Context(optionsBuilder.Options);
        }
    }
}