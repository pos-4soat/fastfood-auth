namespace fastfood_auth.Constants;

public static class ErrorMessages
{
    public static Dictionary<string, string> ErrorMessageList => _errorMessages;

    private static readonly Dictionary<string, string> _errorMessages = new()
    {
        { "ABE001", "Já existe um usuário cadastrado com esse CPF ou e-mail" },
        { "ABE002", "Usuário não encontrado para esse CPF" },
        { "ABE003", "Falha durante login anônimo" },
        { "ABE004", "O nome deve estar preenchido." },
        { "ABE005", "O nome deve ter no minimo 3 e no máximo 255 caracteres." },
        { "ABE006", "O e-mail deve estar preenchido." },
        { "ABE007", "O e-mail fornecido não é válido." },
        { "ABE008", "O cpf deve estar preenchido." },
        { "ABE009", "O cpf deve ser válido" },
        { "ABI998", "Request inválido ou fora de especificação, vide documentação" },
        { "ABI999", "Internal server error" }
    };
}
