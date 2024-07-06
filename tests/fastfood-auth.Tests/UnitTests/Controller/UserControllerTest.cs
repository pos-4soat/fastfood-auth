using fastfood_auth.API.Controllers;
using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Application.UseCases.DeleteUser;
using fastfood_auth.Application.UseCases.GetUser;
using fastfood_auth.Application.UseCases.GuestAuth;
using fastfood_auth.Application.UseCases.UserAuth;
using fastfood_auth.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace fastfood_auth.Tests.UnitTests.Controller;

public class UserControllerTest : TestFixture
{
    [Test, Description("")]
    public async Task ShouldAuthenticateUser()
    {
        UserAuthRequest request = _modelFakerFactory.GenerateRequest<UserAuthRequest>();

        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<UserAuthRequest>(), default))
            .ReturnsAsync(Result<UserAuthResponse>.Success(_modelFakerFactory.GenerateRequest<UserAuthResponse>()));

        UserController service = new(_mediatorMock.Object);

        IActionResult result = await service.AuthenticateUser(request.cpf, default);

        AssertExtensions.AssertResponse<UserAuthRequest, UserAuthResponse>(result, HttpStatusCode.OK, nameof(StatusResponse.SUCCESS), request);
    }

    [Test, Description("")]
    public async Task ShouldAuthenticateAsGuest()
    {
        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<GuestAuthRequest>(), default))
            .ReturnsAsync(Result<GuestAuthResponse>.Success(new GuestAuthResponse()
            {
                Token = Faker.Lorem.Text()
            }));

        UserController service = new(_mediatorMock.Object);

        IActionResult result = await service.AuthenticateAsGuest(default);

        AssertExtensions.AssertResponse<GuestAuthRequest, GuestAuthResponse>(result, HttpStatusCode.OK, nameof(StatusResponse.SUCCESS), null);
    }

    [Test, Description("")]
    public async Task ShouldGetUsers()
    {
        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetUsersRequest>(), default))
            .ReturnsAsync(Result<GetUsersResponse>.Success(new GetUsersResponse()));

        UserController service = new(_mediatorMock.Object);

        IActionResult result = await service.GetUsers(default);

        AssertExtensions.AssertResponse<GetUsersRequest, GetUsersResponse>(result, HttpStatusCode.OK, nameof(StatusResponse.SUCCESS), null);
    }

    [Test, Description("")]
    public async Task ShouldReturnProductNotFoundOnDeleteProductAsync()
    {
        UserAuthRequest request = _modelFakerFactory.GenerateRequest<UserAuthRequest>();

        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<UserAuthRequest>(), default))
            .ReturnsAsync(Result<UserAuthResponse>.Failure("ABE009"));

        UserController service = new(_mediatorMock.Object);

        IActionResult result = await service.AuthenticateUser(request.cpf, default);

        AssertExtensions.AssertErrorResponse(result, HttpStatusCode.BadRequest, nameof(StatusResponse.ERROR));
    }

    [Test, Description("")]
    public async Task ShouldDeleteUserAsync()
    {
        DeleteUserRequest request = _modelFakerFactory.GenerateRequest<DeleteUserRequest>();

        Mock<IMediator> _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteUserRequest>(), default))
            .ReturnsAsync(Result<DeleteUserResponse>.Success(new DeleteUserResponse()));

        UserController service = new(_mediatorMock.Object);

        IActionResult result = await service.DeleteUser(request, default);

        AssertExtensions.AssertResponse<DeleteUserRequest, DeleteUserResponse>(result, HttpStatusCode.OK, nameof(StatusResponse.SUCCESS), null);
    }
}
