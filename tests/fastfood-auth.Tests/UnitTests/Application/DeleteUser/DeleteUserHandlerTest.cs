using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Application.UseCases.CreateUser;
using fastfood_auth.Application.UseCases.DeleteUser;
using fastfood_auth.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.UnitTests.Application.DeleteUser;

public class DeleteUserHandlerTest : TestFixture
{
    [Test, Description("Should return user deleted successfully")]
    public async Task ShouldDeleteUserAsync()
    {
        DeleteUserRequest request = _modelFakerFactory.GenerateRequest<DeleteUserRequest>();

        _repositoryMock.SetupGetUserByCPFOrEmailAsync(_modelFakerFactory.GenerateRequest<UserEntity>());

        DeleteUserHandler service = new(_repositoryMock.Object, _mapper, _userCreationMock.Object);

        Result<DeleteUserResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsSuccess(result, HttpStatusCode.OK);

        _repositoryMock.VerifyGetUserByCPFOrEmailAsync();
        _repositoryMock.VerifyDeleteUserAsync();
        _repositoryMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyDeleteUser();
        _userCreationMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should return user not found")]
    public async Task ShouldReturnUserNotFound()
    {
        DeleteUserRequest request = _modelFakerFactory.GenerateRequest<DeleteUserRequest>();

        _repositoryMock.SetupGetUserByCPFOrEmailAsync(null);

        DeleteUserHandler service = new(_repositoryMock.Object, _mapper, _userCreationMock.Object);

        Result<DeleteUserResponse> result = await service.Handle(request, default);

        AssertExtensions.ResultIsFailure(result, "ABE009", HttpStatusCode.BadRequest);

        _repositoryMock.VerifyGetUserByCPFOrEmailAsync();
        _repositoryMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }
}
