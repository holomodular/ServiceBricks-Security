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
        /// OnModelCreating.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // AI: Create the model for each table
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
            });

            builder.Entity<ApplicationUserToken>(b =>
            {
                b.ToTable("UserToken");
                b.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
            });

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("User");
                b.HasKey(key => key.Id);
                b.Property(key => key.Id).HasDefaultValueSql("newsequentialid()");

                b.HasMany(e => e.ApplicationUserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("Role");
                b.HasKey(key => key.Id);
                b.Property(key => key.Id).HasDefaultValueSql("newsequentialid()");

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