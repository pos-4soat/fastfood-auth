using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Domain.Contracts.Repository;
using fastfood_auth.Domain.Entity;
using fastfood_auth.Tests.UnitTests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.Mocks;

public class UserCreationMock : BaseCustomMock<IUserCreation>
{
    public UserCreationMock(TestFixture testFixture) : base(testFixture)
    {
    }

    public void SetupCreateUser(string expectedReturn)
        => Setup(x => x.CreateUser(It.IsAny<UserEntity>(), default))
            .ReturnsAsync(expectedReturn);

    public void VerifyCreateUser(Times? times = null)
        => Verify(x => x.CreateUser(It.IsAny<UserEntity>(), default), times ?? Times.Once());
}