using IdentityModel;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordOwnerCredentialExample.Validators
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.UserName == "test-user1" && context.Password == "12345")
                context.Result = new GrantValidationResult("test-user1", OidcConstants.AuthenticationMethods.Password);
        }
    }
}
