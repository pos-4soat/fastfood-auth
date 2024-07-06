using Amazon.CognitoIdentityProvider.Model;
using fastfood_auth.Domain.Entity;
using fastfood_auth.Infra.Cognito.Authentication;
using fastfood_auth.Infra.Cognito.Creation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.UnitTests.Infrastructure.Cognito;

public class CognitoUserCreationTest : TestFixture
{
    [Test, Description("Should create user successfully")]
    public async Task ShouldCreateUserAsync()
    {
        var entity = _modelFakerFactory.GenerateRequest<UserEntity>();

        _cognitoMock.SetupAdminSetUserPasswordAsync(_modelFakerFactory.GenerateRequest<AdminSetUserPasswordResponse>());
        _cognitoMock.SetupAdminCreateUserAsync(_modelFakerFactory.GenerateRequest<AdminCreateUserResponse>());

        CognitoUserCreation service = new(_cognitoMock.Object);

        string result = await service.CreateUser(entity, default);

        Assert.True(!string.IsNullOrEmpty(result));

        _repositoryMock.VerifyNoOtherCalls();
        _userAuthenticationMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should delete user successfully")]
    public async Task ShouldDeleteUserAsync()
    {
        var entity = _modelFakerFactory.GenerateRequest<UserEntity>();

        _cognitoMock.SetupAdminDeleteUserAsync(_modelFakerFactory.GenerateRequest<AdminDeleteUserResponse>());

        CognitoUserCreation service = new(_cognitoMock.Object);

        var result = service.DeleteUser(entity, default);

        Assert.True(result.IsCompletedSuccessfully);

        _repositoryMock.VerifyNoOtherCalls();
        _userAuthenticationMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }
}
