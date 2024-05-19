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

public class UserRepositoryMock : BaseCustomMock<IUserRepository>
{
    public UserRepositoryMock(TestFixture testFixture) : base(testFixture)
    {
    }

    public void SetupAddUserAsync(bool expectedReturn)
        => Setup(x => x.AddUserAsync(It.IsAny<UserEntity>(), default))
            .ReturnsAsync(expectedReturn);

    public void SetupGetUsersAsync(IEnumerable<UserEntity> expectedReturn)
        => Setup(x => x.GetUsersAsync(default))
            .ReturnsAsync(expectedReturn);

    public void SetupGetUserByCPFOrEmailAsync(UserEntity expectedReturn)
        => Setup(x => x.GetUserByCPFOrEmailAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(expectedReturn);

    public void VerifyAddUserAsync(Times? times = null)
        => Verify(x => x.AddUserAsync(It.IsAny<UserEntity>(), default), times ?? Times.Once());

    public void VerifyGetUsersAsync(Times? times = null)
        => Verify(x => x.GetUsersAsync(default), times ?? Times.Once());

    public void VerifyGetUserByCPFOrEmailAsync(Times? times = null)
        => Verify(x => x.GetUserByCPFOrEmailAsync(It.IsAny<string>(), It.IsAny<string>(), default), times ?? Times.Once());
}
