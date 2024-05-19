using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Domain.Entity;
using fastfood_auth.Tests.UnitTests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.Mocks;

public class UserAuthenticationMock : BaseCustomMock<IUserAuthentication>
{
    public UserAuthenticationMock(TestFixture testFixture) : base(testFixture)
    {
    }

    public void SetupAuthenticateUser(string expectedReturn)
        => Setup(x => x.AuthenticateUser(It.IsAny<UserEntity>(), default))
            .ReturnsAsync(expectedReturn);

    public void VerifyAuthenticateUser(Times? times = null)
        => Verify(x => x.AuthenticateUser(It.IsAny<UserEntity>(), default), times ?? Times.Once());
}