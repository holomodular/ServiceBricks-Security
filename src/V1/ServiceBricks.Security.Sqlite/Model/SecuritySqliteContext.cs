using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using ServiceBricks.Security.EntityFrameworkCore;
using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.Sqlite
{
    // dotnet ef migrations add SecurityV1 --context SecuritySqliteContext --startup-project ../Test/MigrationsHost

    /// <summary>
    /// This is the database context for the Security module.
    /// </summary>
    public partial class SecuritySqliteContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>, IDesignTimeDbContextFactory<SecuritySqliteContext>
    {
        protected DbContextOptions<SecuritySqliteContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecuritySqliteContext()
        {
            var configBuider = new ConfigurationBuilder();
            configBuider.AddAppSettingsConfig();
            var configuration = configBuider.Build();

            var builder = new DbContextOptionsBuilder<SecuritySqliteContext>();
            string connectionString = configuration.GetSqliteConnectionString(
                SecuritySqliteConstants.APPSETTING_CONNECTION_STRING);
            builder.UseSqlite(connectionString, x =>
            {
                x.MigrationsAssembly(typeof(SecuritySqliteContext).Assembly.GetName().Name);
            });

            _options = builder.Options;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public SecuritySqliteContext(DbContextOptions<SecuritySqliteContext> options) : base(options)
        {
            _options = options;
        }

        /// <summary>
        /// Audit users.
        /// </summary>
        public virtual DbSet<UserAudit> UserAudits { get; set; }

        /// <summary>
        /// Application users.
        /// </summary>
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        /// <summary>
        /// Application user claims
        /// </summary>
        public virtual DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }

        /// <summary>
        /// Application user roles
        /// </summary>
        public virtual DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        /// <summary>
        /// Application user tokens
        /// </summary>
        public virtual DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }

        /// <summary>
        /// Application user logins
        /// </summary>
        public virtual DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }

        /// <summary>
        /// Application roles
        /// </summary>
        public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }

        /// <summary>
        /// Application role claims
        /// </summary>
        public virtual DbSet<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }

        /// <summary>
        /// OnModelCreating.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // AI: Set the default schema
            builder.HasDefaultSchema(SecuritySqliteConstants.DATABASE_SCHEMA_NAME);

            // AI: Setup the entities to the model
            builder.Entity<UserAudit>().HasKey(key => key.Key);
            builder.Entity<UserAudit>().HasIndex(key => new { key.UserId, key.CreateDate });

            builder.Entity<ApplicationUserRole>(b =>
            {
                b.ToTable("UserRole").HasKey(key => new { key.UserId, key.RoleId });
                b.Property<Guid>("UserId");
                b.Property<Guid>("RoleId");
                b.HasOne(x => x.User).WithMany(x => x.ApplicationUserRoles).HasForeignKey(x => x.UserId);
                b.HasOne(x => x.Role).WithMany(x => x.ApplicationUserRoles).HasForeignKey(x => x.RoleId);
            });

            builder.Entity<ApplicationUserClaim>(b =>
            {
                b.ToTable("UserClaim").HasKey(x => x.Id);
            });

            builder.Entity<ApplicationUserLogin>(b =>
            {
                b.ToTable("UserLogin").HasKey(key => new { key.LoginProvider, key.ProviderKey });
            });

            builder.Entity<ApplicationRoleClaim>(b =>
            {
                b.ToTable("RoleClaim").HasKey(key => key.Id);
            });

            builder.Entity<ApplicationUserToken>(b =>
            {
                b.ToTable("UserToken").HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
            });

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("User").HasKey(key => key.Id);

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.ApplicationUserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("Role").HasKey(key => key.Id);

                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.ApplicationUserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });
        }

        /// <summary>
        /// Configure conventions
        /// </summary>
        /// <param name="configurationBuilder"></param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<DateTimeOffset>()
                .HaveConversion<DateTimeOffsetToBytesConverter>();
            configurationBuilder
                .Properties<DateTimeOffset?>()
                .HaveConversion<DateTimeOffsetToBytesConverter>();
        }

        /// <summary>
        /// Create context.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual SecuritySqliteContext CreateDbContext(string[] args)
        {
            return new SecuritySqliteContext(_options);
        }
    }
}