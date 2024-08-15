using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.InMemory
{
    /// <summary>
    /// This is the database context for the Security module.
    /// </summary>
    public partial class SecurityInMemoryContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>, IDesignTimeDbContextFactory<SecurityInMemoryContext>
    {
        protected DbContextOptions<SecurityInMemoryContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecurityInMemoryContext()
        {
            var configBuider = new ConfigurationBuilder();
            configBuider.AddAppSettingsConfig();
            var configuration = configBuider.Build();

            var builder = new DbContextOptionsBuilder<SecurityInMemoryContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            _options = builder.Options;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public SecurityInMemoryContext(DbContextOptions<SecurityInMemoryContext> options) : base(options)
        {
            _options = options;
        }

        /// <summary>
        /// The audit users.
        /// </summary>
        public virtual DbSet<UserAudit> UserAudits { get; set; }

        /// <summary>
        /// The application users.
        /// </summary>
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        /// <summary>
        /// The application roles.
        /// </summary>
        public virtual DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }

        /// <summary>
        /// The application user roles.
        /// </summary>
        public virtual DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        /// <summary>
        /// The application user tokens
        /// </summary>
        public virtual DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }

        /// <summary>
        /// The application user logins.
        /// </summary>
        public virtual DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }

        /// <summary>
        /// The application roles.
        /// </summary>
        public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }

        /// <summary>
        /// The application role claims.
        /// </summary>
        public virtual DbSet<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }

        /// <summary>
        /// OnModelCreating.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // AI: Create the model for each table
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
                b.ToTable("User").Property(key => key.Id).HasDefaultValueSql("newsequentialid()");

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.ApplicationUserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("Role").Property(key => key.Id).HasDefaultValueSql("newsequentialid()");

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
        public virtual SecurityInMemoryContext CreateDbContext(string[] args)
        {
            return new SecurityInMemoryContext(_options);
        }
    }
}