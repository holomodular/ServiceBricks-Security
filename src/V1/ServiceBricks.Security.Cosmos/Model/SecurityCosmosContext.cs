using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ServiceBricks.Storage.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.Cosmos
{
    // dotnet ef migrations add SecurityV1 --context SecurityCosmosContext --startup-project ../Tests/WebApp

    /// <summary>
    /// This is the database context for the Security module.
    /// </summary>
    public partial class SecurityCosmosContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>, IDesignTimeDbContextFactory<SecurityCosmosContext>
    {
        protected DbContextOptions<SecurityCosmosContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityCosmosContext()
        {
            var configBuider = new ConfigurationBuilder();
            configBuider.AddAppSettingsConfig();
            var configuration = configBuider.Build();

            var builder = new DbContextOptionsBuilder<SecurityCosmosContext>();
            string connectionString = configuration.GetCosmosConnectionString(
                SecurityCosmosConstants.APPSETTING_CONNECTION_STRING);
            string database = configuration.GetCosmosDatabase(
                SecurityCosmosConstants.APPSETTING_DATABASE);
            builder.UseCosmos(connectionString, database);
            _options = builder.Options;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public SecurityCosmosContext(DbContextOptions<SecurityCosmosContext> options) : base(options)
        {
            _options = options;
        }

        public virtual DbSet<Cosmos.AuditUser> AuditUsers { get; set; }
        public virtual DbSet<Cosmos.ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Cosmos.ApplicationUserClaim> ApplicationUserClaims { get; set; }
        public virtual DbSet<Cosmos.ApplicationUserRole> ApplicationUserRoles { get; set; }
        public virtual DbSet<Cosmos.ApplicationUserToken> ApplicationUserTokens { get; set; }
        public virtual DbSet<Cosmos.ApplicationUserLogin> ApplicationUserLogins { get; set; }
        public virtual DbSet<Cosmos.ApplicationRole> ApplicationRoles { get; set; }
        public virtual DbSet<Cosmos.ApplicationRoleClaim> ApplicationRoleClaims { get; set; }

        /// <summary>
        /// OnModelCreating.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Set default schema
            builder.HasDefaultSchema(SecurityCosmosConstants.DATABASE_SCHEMA_NAME);

            builder.Entity<AuditUser>().HasKey(key => key.Key);
            builder.Entity<AuditUser>().HasIndex(key => new { key.UserId, key.CreateDate });

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
                b.ToTable("UserClaim").HasKey(x => x.Key);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
            });

            builder.Entity<ApplicationUserLogin>(b =>
            {
                b.ToTable("UserLogin").HasKey(key => key.Key);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
            });

            builder.Entity<ApplicationRoleClaim>(b =>
            {
                b.ToTable("RoleClaim").HasKey(key => key.Key);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
            });

            builder.Entity<ApplicationUserToken>(b =>
            {
                b.ToTable("UserToken").HasKey(key => key.Key);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
            });

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("User").Property(key => key.Id).ValueGeneratedOnAdd();
                b.Property(x => x.ConcurrencyStamp).IsETagConcurrency();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.ApplicationUserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("Role").Property(key => key.Id).ValueGeneratedOnAdd();
                b.Property(x => x.ConcurrencyStamp).IsETagConcurrency();
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.ApplicationUserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });
        }

        /// <summary>
        /// Create context.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual SecurityCosmosContext CreateDbContext(string[] args)
        {
            return new SecurityCosmosContext(_options);
        }
    }
}