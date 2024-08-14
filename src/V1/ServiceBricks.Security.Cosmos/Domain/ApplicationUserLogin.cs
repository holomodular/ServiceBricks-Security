﻿using Microsoft.AspNetCore.Identity;
using ServiceBricks.Storage.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceBricks.Security.Cosmos
{
    /// <summary>
    /// This is an external login for a user.
    /// </summary>
    public partial class ApplicationUserLogin : IdentityUserLogin<Guid>, IEntityFrameworkCoreDomainObject<ApplicationUserLogin>
    {
        /// <summary>
        /// Internal primary key.
        /// </summary>
        public virtual Guid Key { get; set; }

        /// <summary>
        /// Provide any defaults for the IQueryable object.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IQueryable<ApplicationUserLogin> DomainGetIQueryableDefaults(IQueryable<ApplicationUserLogin> query)
        {
            return query;
        }

        /// <summary>
        /// Provide an expression that will filter an object based on its primary key.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Expression<Func<ApplicationUserLogin, bool>> DomainGetItemFilter(ApplicationUserLogin obj)
        {
            return x => x.Key == obj.Key;
        }
    }
}