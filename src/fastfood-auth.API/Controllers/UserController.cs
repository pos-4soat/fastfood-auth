using fastfood_auth.API.Controllers.Base;
using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Application.UseCases.CreateUser;
using fastfood_auth.Application.UseCases.GetUser;
using fastfood_auth.Application.UseCases.GuestAuth;
using fastfood_auth.Application.UseCases.UserAuth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace fastfood_auth.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IMediator _mediator) : BaseController
{
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser(CreateUserRequest customerCreateRequestDto, CancellationToken cancellationToken)
    {
        Result<CreateUserResponse> result = await _mediator.Send(customerCreateRequestDto, cancellationToken);
        return await GetResponseFromResult(result);
    }

    [HttpGet("AuthenticateUser/{cpf}")]
    public async Task<IActionResult> AuthenticateUser(string cpf, CancellationToken cancellationToken)
    {
        Result<UserAuthResponse> result = await _mediator.Send(new UserAuthRequest(cpf), cancellationToken);
        return await GetResponseFromResult(result);
    }

    [HttpGet("AuthenticateAsGuest")]
    public async Task<IActionResult> AuthenticateAsGuest(CancellationToken cancellationToken)
    {
        Result<GuestAuthResponse> result = await _mediator.Send(new GuestAuthRequest(), cancellationToken);
        return await GetResponseFromResult(result);
    }

    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        Result<GetUsersResponse> result = await _mediator.Send(new GetUsersRequest(), cancellationToken);
        return await GetResponseFromResult(result);
    }
}
