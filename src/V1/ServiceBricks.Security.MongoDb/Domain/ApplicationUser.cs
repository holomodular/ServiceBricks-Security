using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.MongoDb;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceBricks.Security.MongoDb
{
    /// <summary>
    /// This is a user in the application.
    /// </summary>
    public partial class ApplicationUser : IMongoDbDomainObject<ApplicationUser>, IDpCreateDate, IDpUpdateDate
    {
        //
        // Summary:
        //     Gets or sets the primary key for this user.
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        //
        // Summary:
        //     Gets or sets the date and time, in UTC, when any user lockout ends.
        //
        // Remarks:
        //     A value in the past means the user is not locked out.
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        //
        // Summary:
        //     Gets or sets a flag indicating if two factor authentication is enabled for this
        //     user.
        //
        // Value:
        //     True if 2fa is enabled, otherwise false.
        [PersonalData]
        public virtual bool TwoFactorEnabled { get; set; }

        //
        // Summary:
        //     Gets or sets a flag indicating if a user has confirmed their telephone address.
        //
        // Value:
        //     True if the telephone number has been confirmed, otherwise false.
        [PersonalData]
        public virtual bool PhoneNumberConfirmed { get; set; }

        //
        // Summary:
        //     Gets or sets a telephone number for the user.
        [ProtectedPersonalData]
        public virtual string PhoneNumber { get; set; }

        //
        // Summary:
        //     A random value that must change whenever a user is persisted to the store
        public virtual string ConcurrencyStamp { get; set; }

        //
        // Summary:
        //     A random value that must change whenever a users credentials change (password
        //     changed, login removed)
        public virtual string SecurityStamp { get; set; }

        //
        // Summary:
        //     Gets or sets a salted and hashed representation of the password for this user.
        public virtual string PasswordHash { get; set; }

        //
        // Summary:
        //     Gets or sets a flag indicating if a user has confirmed their email address.
        //
        // Value:
        //     True if the email address has been confirmed, otherwise false.
        [PersonalData]
        public virtual bool EmailConfirmed { get; set; }

        //
        // Summary:
        //     Gets or sets the normalized email address for this user.
        public virtual string NormalizedEmail { get; set; }

        //
        // Summary:
        //     Gets or sets the email address for this user.
        [ProtectedPersonalData]
        public virtual string Email { get; set; }

        //
        // Summary:
        //     Gets or sets the normalized user name for this user.
        public virtual string NormalizedUserName { get; set; }

        //
        // Summary:
        //     Gets or sets the user name for this user.
        [ProtectedPersonalData]
        public virtual string UserName { get; set; }

        //
        // Summary:
        //     Gets or sets a flag indicating if the user could be locked out.
        //
        // Value:
        //     True if the user could be locked out, otherwise false.
        public virtual bool LockoutEnabled { get; set; }

        //
        // Summary:
        //     Gets or sets the number of failed login attempts for the current user.
        public virtual int AccessFailedCount { get; set; }

        public virtual DateTimeOffset CreateDate { get; set; }
        public virtual DateTimeOffset UpdateDate { get; set; }

        public virtual Expression<Func<ApplicationUser, bool>> DomainGetItemFilter(ApplicationUser obj)
        {
            return x => x.Id == obj.Id;
        }
    }
}