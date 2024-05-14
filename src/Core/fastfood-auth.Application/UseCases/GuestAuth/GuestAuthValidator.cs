using FluentValidation;

namespace fastfood_auth.Application.UseCases.GuestAuth;

public class GuestAuthValidator : AbstractValidator<GuestAuthRequest>
{
    public GuestAuthValidator()
    {

    }
}
