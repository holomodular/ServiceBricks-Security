using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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

        /// <summary>
        /// Audit users.
        /// </summary>
        public virtual DbSet<UserAudit> UserAudits { get; set; }

        /// <summary>
        /// OnModelCreating.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // AI: Set the default container name
            //builder.Model.SetDefaultContainer(SecurityCosmosConstants.CONTAINER_PREFIX);

            // AI: Create the model for each table
            builder.Entity<UserAudit>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(UserAudit)));
                b.ToTable("UserAudit");
                b.HasKey(key => key.Key);
                b.HasPartitionKey(key => key.UserId);
                b.Property(key => key.Key).ValueGeneratedOnAdd();
                b.HasIndex(key => new { key.UserId, key.AuditType, key.CreateDate });
            });

            builder.Entity<ApplicationUserRole>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationUserRole)));
                b.ToTable("UserRole");
                b.HasKey(key => new { key.UserId, key.RoleId });
                b.HasPartitionKey(x => x.UserId);
                b.Property<Guid>("UserId");
                b.Property<Guid>("RoleId");
            });

            builder.Entity<ApplicationUserClaim>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationUserClaim)));
                b.ToTable("UserClaim");
                b.HasKey(x => x.Key);
                b.HasPartitionKey(x => x.UserId);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
            });

            builder.Entity<ApplicationUserLogin>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationUserLogin)));
                b.ToTable("UserLogin");
                b.HasKey(key => key.Key);
                b.HasPartitionKey(x => x.UserId);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
            });

            builder.Entity<ApplicationRoleClaim>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationRoleClaim)));
                b.ToTable("RoleClaim");
                b.HasKey(key => key.Key);
                b.HasPartitionKey(x => x.RoleId);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
            });

            builder.Entity<ApplicationUserToken>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationUserToken)));
                b.ToTable("UserToken");
                b.HasKey(key => key.Key);
                b.HasPartitionKey(x => x.UserId);
                b.Property(x => x.Key).ValueGeneratedOnAdd();
            });

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationUser)));
                b.ToTable("User");
                b.HasKey(x => x.Id);
                b.Property(key => key.Id).ValueGeneratedOnAdd();
                b.HasPartitionKey(x => x.Id);
                b.Property(x => x.ConcurrencyStamp).IsETagConcurrency();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToContainer(SecurityCosmosConstants.GetContainerName(nameof(ApplicationRole)));
                b.ToTable("Role");
                b.HasKey(x => x.Id);
                b.HasPartitionKey(x => x.Id);
                b.Property(key => key.Id).ValueGeneratedOnAdd();
                b.Property(x => x.ConcurrencyStamp).IsETagConcurrency();
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