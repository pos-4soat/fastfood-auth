namespace fastfood_auth.Domain.Contants;

public static class ErrorMessages
{
    public static Dictionary<string, string> ErrorMessageList => _errorMessages;

    private static readonly Dictionary<string, string> _errorMessages = new()
    {
        { "ABE001", "Request inválido ou fora de especificação, vide documentação." },
        { "ABE002", "O nome deve estar preenchido." },
        { "ABE003", "O nome deve ter no minimo 3 e no máximo 255 caracteres." },
        { "ABE004", "O e-mail deve estar preenchido." },
        { "ABE005", "O e-mail fornecido não é válido." },
        { "ABE006", "O cpf deve estar preenchido." },
        { "ABE007", "O cpf deve ser válido." },
        { "ABE008", "Já existe um usuário cadastrado com esse CPF ou e-mail." },
        { "ABE009", "Usuário não encontrado para esse CPF." },
        { "AIE999", "Internal server error" }
    };
}
