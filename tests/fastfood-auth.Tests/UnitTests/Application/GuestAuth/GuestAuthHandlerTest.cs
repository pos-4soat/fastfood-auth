using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Application.UseCases.GetUser;
using fastfood_auth.Application.UseCases.GuestAuth;
using fastfood_auth.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.UnitTests.Application.GuestAuth;

public class GuestAuthHandlerTest : TestFixture
{
    [Test, Description("Should return guest token successfully")]
    public async Task ShouldReturnGuestTokenAsync()
    {
        GuestAuthRequest request = _modelFakerFactory.GenerateRequest<GuestAuthRequest>();
        var token = Faker.Lorem.Word();

        _userAuthenticationMock.SetupAuthenticateUser(token);

        GuestAuthHandler service = new(_userAuthenticationMock.Object);

        Result<GuestAuthResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.OK);

        Assert.That(result.Value.Token, Is.EqualTo(token));

        _userAuthenticationMock.VerifyAuthenticateUser();
        _userAuthenticationMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }
}