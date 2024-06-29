using fastfood_auth.Application.Shared.BaseResponse;
using MediatR;

namespace fastfood_auth.Application.UseCases.CreateUser;

public sealed record CreateUserRequest(string Name, string Email, string Phone, string Identification) :
    IRequest<Result<CreateUserResponse>>;
