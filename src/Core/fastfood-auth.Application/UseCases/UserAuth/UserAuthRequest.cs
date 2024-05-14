using fastfood_auth.Application.Shared.BaseResponse;
using MediatR;

namespace fastfood_auth.Application.UseCases.UserAuth;

public sealed record UserAuthRequest(string cpf) :
 IRequest<Result<UserAuthResponse>>;
