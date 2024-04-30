using fastfood_auth.Models.Request;
using FluentValidation;

namespace fastfood_auth.Validations;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        _ = RuleFor(dto => dto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ABE004")
            .Length(3, 255)
            .WithMessage("ABE005");

        _ = RuleFor(dto => dto.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ABE006")
            .EmailAddress()
            .WithMessage("ABE007");

        _ = RuleFor(dto => dto.Identification)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ABE008")
            .Must(IsValidCpf)
            .WithMessage("ABE009");
    }

    private bool IsValidCpf(string cpf)
    {
        int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
        string tempCpf;
        string digito;
        int soma;
        int resto;
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;
        tempCpf = cpf[..9];
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito = resto.ToString();
        tempCpf += digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito += resto.ToString();

        return cpf.EndsWith(digito);
    }
}