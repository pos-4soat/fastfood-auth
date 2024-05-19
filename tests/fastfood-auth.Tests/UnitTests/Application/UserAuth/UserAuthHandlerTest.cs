using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Application.UseCases.GetUser;
using fastfood_auth.Application.UseCases.UserAuth;
using fastfood_auth.Domain.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.UnitTests.Application.UserAuth;

public class UserAuthHandlerTest : TestFixture
{
    [Test, Description("Should return user token successfully")]
    public async Task ShouldReturnUserTokenAsync()
    {
        UserAuthRequest request = _modelFakerFactory.GenerateRequest<UserAuthRequest>();

        var user = _modelFakerFactory.GenerateRequest<UserEntity>();
        var token = Faker.Lorem.Word();

        _repositoryMock.SetupGetUserByCPFOrEmailAsync(user);
        _userAuthenticationMock.SetupAuthenticateUser(token);

        UserAuthHandler service = new(_repositoryMock.Object, _mapper, _userAuthenticationMock.Object);

        Result<UserAuthResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.OK);

        Assert.That(result.Value.Name, Is.EqualTo(user.Name));
        Assert.That(result.Value.Email, Is.EqualTo(user.Email));
        Assert.That(result.Value.Identification, Is.EqualTo(user.Identification));
        Assert.That(result.Value.Token, Is.EqualTo(token));

        _repositoryMock.VerifyGetUserByCPFOrEmailAsync();
        _repositoryMock.VerifyNoOtherCalls();
        _userAuthenticationMock.VerifyAuthenticateUser();
        _userAuthenticationMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return user not found successfully")]
    public async Task ShouldReturnUserNotFoundAsync()
    {
        UserAuthRequest request = _modelFakerFactory.GenerateRequest<UserAuthRequest>();

        _repositoryMock.SetupGetUserByCPFOrEmailAsync(null);

        UserAuthHandler service = new(_repositoryMock.Object, _mapper, _userAuthenticationMock.Object);

        Result<UserAuthResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "ABE009");

        _repositoryMock.VerifyGetUserByCPFOrEmailAsync();
        _repositoryMock.VerifyNoOtherCalls();
        _userAuthenticationMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }
}
