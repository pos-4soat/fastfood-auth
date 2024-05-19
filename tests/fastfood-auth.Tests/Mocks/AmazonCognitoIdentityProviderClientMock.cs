using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using fastfood_auth.Domain.Entity;
using fastfood_auth.Tests.UnitTests;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.Mocks;

public class AmazonCognitoIdentityProviderClientMock : Mock<AmazonCognitoIdentityProviderClient>
{
    public AmazonCognitoIdentityProviderClientMock() : base()
    {
    }

    public void SetupAdminInitiateAuthAsync(AdminInitiateAuthResponse expectedReturn)
        => Setup(c => c.AdminInitiateAuthAsync(It.IsAny<AdminInitiateAuthRequest>(), default))
            .ReturnsAsync(expectedReturn);

    public void SetupAdminSetUserPasswordAsync(AdminSetUserPasswordResponse expectedReturn)
        => Setup(c => c.AdminSetUserPasswordAsync(It.IsAny<AdminSetUserPasswordRequest>(), default))
            .ReturnsAsync(expectedReturn);

    public void SetupAdminCreateUserAsync(AdminCreateUserResponse expectedReturn)
        => Setup(c => c.AdminCreateUserAsync(It.IsAny<AdminCreateUserRequest>(), default))
            .ReturnsAsync(expectedReturn);
}