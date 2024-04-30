using fastfood_auth.Controllers.Base;
using fastfood_auth.Interface;
using fastfood_auth.Models.Base;
using fastfood_auth.Models.Request;
using fastfood_auth.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace fastfood_auth.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("[controller]")]
public class UserController(IUserService _service) : BaseController()
{
    [HttpPost("CreateUser")]
    [SwaggerOperation(Summary = "Create a new user")]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Error handled by the application", typeof(ErrorResponse<Error>))]
    [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<CreateUserResponse>))]
    public async Task<IActionResult> CreateUser(CreateUserRequest customerCreateRequestDto, CancellationToken cancellationToken)
    {
        Result<CreateUserResponse> result = await _service.CreateAsync(customerCreateRequestDto, cancellationToken);
        return await GetResponseFromResult(result);
    }

    [HttpGet("AuthenticateUser/{cpf}")]
    [SwaggerOperation(Summary = "Authenticate user")]
    [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<>))]
    //[SwaggerResponse((int)HttpStatusCode.BadRequest, "Error handled by the application", typeof(TopupCreateErrorResponse))]
    public async Task<IActionResult> AuthenticateUser(string cpf, CancellationToken cancellationToken)
    {
        Result<AuthenticateUserResponse> result = await _service.AuthenticateAsync(cpf, cancellationToken);
        return await GetResponseFromResult(result);
    }

    [HttpGet("AuthenticateAsGuest")]
    [SwaggerOperation(Summary = "Authenticate as guest")]
    [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<>))]
    //[SwaggerResponse((int)HttpStatusCode.BadRequest, "Error handled by the application", typeof(TopupCreateErrorResponse))]
    public async Task<IActionResult> AuthenticateAsGuest(CancellationToken cancellationToken)
    {
        Result<AuthenticateAsGuestResponse> result = await _service.AnonymousAuthenticateAsync(cancellationToken);
        return await GetResponseFromResult(result);
    }

    [HttpGet("GetUsers")]
    [SwaggerOperation(Summary = "List all users")]
    [SwaggerResponse((int)HttpStatusCode.OK, "OK", typeof(Response<>))]
    //[SwaggerResponse((int)HttpStatusCode.BadRequest, "Error handled by the application", typeof(TopupCreateErrorResponse))]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        Result<GetUsersResponse> result = await _service.GetUsersAsync(cancellationToken);
        return await GetResponseFromResult(result);
    }
}
