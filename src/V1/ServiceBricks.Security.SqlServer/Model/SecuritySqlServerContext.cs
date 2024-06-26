﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ServiceBricks.Storage.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceBricks.Security.EntityFrameworkCore;

namespace ServiceBricks.Security.SqlServer
{
    // dotnet ef migrations add SecurityV1 --context SecuritySqlServerContext --startup-project ../Test/MigrationsHost

    /// <summary>
    /// This is the database context for the Security module.
    /// </summary>
    public partial class SecuritySqlServerContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>, IDesignTimeDbContextFactory<SecuritySqlServerContext>
    {
        protected DbContextOptions<SecuritySqlServerContext> _options = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SecuritySqlServerContext()
        {
            var configBuider = new ConfigurationBuilder();
            configBuider.AddAppSettingsConfig();
            var configuration = configBuider.Build();

            var builder = new DbContextOptionsBuilder<SecuritySqlServerContext>();
            string connectionString = configuration.GetSqlServerConnectionString(
                SecuritySqlServerConstants.APPSETTING_CONNECTION_STRING);
            builder.UseSqlServer(connectionString, x =>
            {
                x.MigrationsAssembly(typeof(SecuritySqlServerContext).Assembly.GetName().Name);
                x.EnableRetryOnFailure();
            });
            _options = builder.Options;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public SecuritySqlServerContext(DbContextOptions<SecuritySqlServerContext> options) : base(options)
        {
            _options = options;
        }

        public virtual DbSet<AuditUser> AuditUsers { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
        public virtual DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public virtual DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }
        public virtual DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }
        public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public virtual DbSet<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }

        /// <summary>
        /// OnModelCreating.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Set default schema
            builder.HasDefaultSchema(SecuritySqlServerConstants.DATABASE_SCHEMA_NAME);

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
        public virtual SecuritySqlServerContext CreateDbContext(string[] args)
        {
            return new SecuritySqlServerContext(_options);
        }
    }
}