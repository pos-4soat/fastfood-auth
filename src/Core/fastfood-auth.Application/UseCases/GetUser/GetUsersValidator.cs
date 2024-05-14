using FluentValidation;

namespace fastfood_auth.Application.UseCases.GetUser;

public class GetUsersValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersValidator()
    {

    }
}
