using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Loja.Core.Identity.Models;
using Loja.Core.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Loja.Core.Identity.Managers
{
    public class AppUserManager<TUser> : UserManager<TUser> where TUser : User
    {
        private readonly IUserStoreExtension<TUser> _storeExtension;

        public AppUserManager(IUserStoreExtension<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer,
                errors, services, logger)
        {
            _storeExtension = store;
        }

        public async Task<IdentityResult> DeactivateAsync(long userId)
        {
            ThrowIfDisposed();
            if (userId == 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await _storeExtension.DeactivateAsync(userId, CancellationToken.None);
        }
    }
}