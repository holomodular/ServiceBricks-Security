using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using ServiceBricks.Security.EntityFrameworkCore;
using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.Sqlite
{
    // dotnet ef migrations add SecurityV1 --context SecuritySqliteContext --startup-project ../Tests/MigrationsHost

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
        public virtual DbSet<UserAudit> UserAudit { get; set; }

        /// <summary>
        /// OnModelCreating.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // AI: Setup the entities to the model
            builder.Entity<UserAudit>(b =>
            {
                b.ToTable("UserAudit");
                b.HasKey(key => key.Key);
                b.Property(key => key.Key).ValueGeneratedOnAdd();
                b.HasIndex(key => new { key.UserId, key.AuditType, key.CreateDate });
            });

            builder.Entity<ApplicationUserRole>(b =>
            {
                b.ToTable("UserRole");
                b.HasKey(key => new { key.UserId, key.RoleId });
                b.Property<Guid>("UserId");
                b.Property<Guid>("RoleId");
                b.HasOne(x => x.User).WithMany(x => x.ApplicationUserRoles).HasForeignKey(x => x.UserId);
                b.HasOne(x => x.Role).WithMany(x => x.ApplicationUserRoles).HasForeignKey(x => x.RoleId);
            });

            builder.Entity<ApplicationUserClaim>(b =>
            {
                b.ToTable("UserClaim");
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            builder.Entity<ApplicationUserLogin>(b =>
            {
                b.ToTable("UserLogin");
                b.HasKey(key => new { key.LoginProvider, key.ProviderKey });
            });

            builder.Entity<ApplicationRoleClaim>(b =>
            {
                b.ToTable("RoleClaim");
                b.HasKey(key => key.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            builder.Entity<ApplicationUserToken>(b =>
            {
                b.ToTable("UserToken");
                b.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
            });

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("User");
                b.HasKey(x => x.Id);
                b.Property(key => key.Id).ValueGeneratedOnAdd();

                b.HasMany(e => e.ApplicationUserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("Role");
                b.HasKey(x => x.Id);
                b.Property(key => key.Id).ValueGeneratedOnAdd();

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