﻿using fastfood_auth.Application.UseCases.CreateUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Application.UseCases.DeleteUser;

public class DeleteUserValidator : AbstractValidator<CreateUserRequest>
{
    public DeleteUserValidator()
    {
        RuleFor(dto => dto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ABE002")
            .Length(3, 255)
            .WithMessage("ABE003");

        RuleFor(dto => dto.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ABE004")
            .EmailAddress()
            .WithMessage("ABE005");

        RuleFor(dto => dto.Identification)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ABE006")
            .Must(BeValidCpf)
            .WithMessage("ABE007");
    }

    private bool BeValidCpf(string cpf)
    {
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digito;
        int soma;
        int resto;
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;
        tempCpf = cpf.Substring(0, 9);
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito = digito + resto.ToString();

        return cpf.EndsWith(digito);
    }
}
