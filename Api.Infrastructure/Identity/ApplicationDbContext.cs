namespace Api.Infrastructure.Identity
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUserProfile, IdentityRole, string>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}