using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Application.UseCases.UserAuth;
using fastfood_auth.Domain.Entity;
using fastfood_auth.Infra.Cognito.Authentication;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.UnitTests.Infrastructure.Cognito;

public class CognitoUserAuthenticationTest : TestFixture
{
    [Test, Description("Should authenticate user successfully")]
    public async Task ShouldAuthenticateUserAsync()
    {
        _memoryCacheMock.SetupTryGetValue(null);
        _cognitoMock.SetupAdminInitiateAuthAsync(_modelFakerFactory.GenerateRequest<AdminInitiateAuthResponse>());

        CognitoUserAuthentication service = new(_cognitoMock.Object, _memoryCacheMock.Object);

        string result = await service.AuthenticateUser(_modelFakerFactory.GenerateRequest<UserEntity>(), default);

        Assert.True(!string.IsNullOrEmpty(result));

        _repositoryMock.VerifyNoOtherCalls();
        _userAuthenticationMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }
}
