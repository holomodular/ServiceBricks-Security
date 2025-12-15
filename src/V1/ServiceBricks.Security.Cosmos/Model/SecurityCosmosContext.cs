using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using ServiceBricks.Storage.EntityFrameworkCore;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is the database context for the Security module.
    /// </summary>
    public partial class SecurityCosmosContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>, IDesignTimeDbContextFactory<SecurityCosmosContext>
    {
        protected DbContextOptions<SecurityCosmosContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public SecurityCosmosContext(DbContextOptions<SecurityCosmosContext> options) : base(options)
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

            // AI: Create the model for each table
            builder.Entity<UserAudit>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(UserAudit)));
                b.ToTable("UserAudit");
                b.HasKey(key => key.Key);
                b.Property(key => key.Key).ValueGeneratedOnAdd();
#if NET8_0_OR_GREATER
#else
                b.HasPartitionKey(x => x.UserId);
#endif
            });

            builder.Entity<ApplicationUserRole>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationUserRole)));
                b.ToTable("UserRole");
                b.HasKey(key => new { key.UserId, key.RoleId });
                b.Property<Guid>("UserId");
                b.Property<Guid>("RoleId");
#if NET8_0_OR_GREATER
#else
                b.HasPartitionKey(x => x.UserId);
#endif
            });

            builder.Entity<ApplicationUserClaim>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationUserClaim)));
                b.ToTable("UserClaim");
                b.HasKey(x => x.Key);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
#if NET8_0_OR_GREATER
#else
                b.HasPartitionKey(x => x.UserId);
#endif
            });

            builder.Entity<ApplicationUserLogin>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationUserLogin)));
                b.ToTable("UserLogin");
                b.HasKey(key => key.Key);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
#if NET8_0_OR_GREATER
#else
                b.HasPartitionKey(x => x.UserId);
#endif
            });

            builder.Entity<ApplicationRoleClaim>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationRoleClaim)));
                b.ToTable("RoleClaim");
                b.HasKey(key => key.Key);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
#if NET8_0_OR_GREATER
#else
                b.HasPartitionKey(x => x.RoleId);
#endif
            });

            builder.Entity<ApplicationUserToken>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationUserToken)));
                b.ToTable("UserToken");
                b.HasKey(key => key.Key);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
#if NET8_0_OR_GREATER
#else
                b.HasPartitionKey(x => x.UserId);
#endif
            });

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationUser)));
                b.ToTable("User");
                b.HasKey(x => x.Id);
                b.Property(key => key.Id).ValueGeneratedOnAdd();
                b.Property(x => x.ConcurrencyStamp).IsETagConcurrency();
#if NET8_0_OR_GREATER
                var indexmeta = b.HasIndex(x => x.NormalizedUserName).Metadata;
                b.Metadata.RemoveIndex(indexmeta);
                indexmeta = b.HasIndex(x => x.NormalizedEmail).Metadata;
                b.Metadata.RemoveIndex(indexmeta);
#else
                b.HasPartitionKey(x => x.Id);
#endif
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationRole)));
                b.ToTable("Role");
                b.HasKey(x => x.Id);
                b.HasPartitionKey(x => x.Id);
                b.Property(key => key.Id).ValueGeneratedOnAdd();
                b.Property(x => x.ConcurrencyStamp).IsETagConcurrency();

#if NET8_0_OR_GREATER
                var indexmeta = b.HasIndex(x => x.NormalizedName).Metadata;
                b.Metadata.RemoveIndex(indexmeta);
#else
                b.HasPartitionKey(x => x.Id);
#endif
            });
        }

        /// <summary>
        /// OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if NET8_0_OR_GREATER
            optionsBuilder.ConfigureWarnings(w => w.Ignore(CosmosEventId.SyncNotSupported));
#endif

            base.OnConfiguring(optionsBuilder);
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