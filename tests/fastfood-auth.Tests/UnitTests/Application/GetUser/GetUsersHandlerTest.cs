using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Application.UseCases.CreateUser;
using fastfood_auth.Application.UseCases.GetUser;
using fastfood_auth.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.UnitTests.Application.GetUser;

public class GetUsersHandlerTest : TestFixture
{
    [Test, Description("Should return users successfully")]
    public async Task ShouldReturnUsersAsync()
    {
        GetUsersRequest request = _modelFakerFactory.GenerateRequest<GetUsersRequest>();
        var entities = _modelFakerFactory.GenerateManyRequest<UserEntity>();

        _repositoryMock.SetupGetUsersAsync(entities);

        GetUsersHandler service = new(_repositoryMock.Object, _mapper);

        Result<GetUsersResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.OK);

        var value = result.Value;

        Assert.That(value.Users.Count(), Is.EqualTo(entities.Count()));

        _repositoryMock.VerifyGetUsersAsync();
        _repositoryMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }
}