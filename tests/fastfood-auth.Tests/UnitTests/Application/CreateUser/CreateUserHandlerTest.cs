using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Application.UseCases.CreateUser;
using fastfood_auth.Domain.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.UnitTests.Application.CreateUser;

public class CreateUserHandlerTest : TestFixture
{
    [Test, Description("Should return user created successfully")]
    public async Task ShouldCreateUserAsync()
    {
        CreateUserRequest request = _modelFakerFactory.GenerateRequest<CreateUserRequest>();
        string cognitoId = Faker.Lorem.Word();

        _repositoryMock.SetupGetUserByCPFOrEmailAsync(null);
        _repositoryMock.SetupAddUserAsync(true);
        _userCreationMock.SetupCreateUser(cognitoId);

        CreateUserHandler service = new(_repositoryMock.Object, _mapper, _userCreationMock.Object);

        Result<CreateUserResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.OK);

        _repositoryMock.VerifyGetUserByCPFOrEmailAsync();
        _repositoryMock.VerifyAddUserAsync();
        _repositoryMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyCreateUser();
        _userCreationMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return user already exists")]
    public async Task ShouldReturnUserAlreadyExists()
    {
        CreateUserRequest request = _modelFakerFactory.GenerateRequest<CreateUserRequest>();

        _repositoryMock.SetupGetUserByCPFOrEmailAsync(_modelFakerFactory.GenerateRequest<UserEntity>());

        CreateUserHandler service = new(_repositoryMock.Object, _mapper, _userCreationMock.Object);

        Result<CreateUserResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "ABE008", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyGetUserByCPFOrEmailAsync();
        _repositoryMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }
}