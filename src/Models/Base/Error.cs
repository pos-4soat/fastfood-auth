﻿using fastfood_auth.Constants;

namespace fastfood_auth.Models.Base;

/// <param name="ErrorCode"> Código de erro. </param>
/// <param name="Message"> Mensagens de erro. </param>
public sealed record Error(string ErrorCode, string Message)
{
    public Error(string errorCode) : this(errorCode, ErrorMessages.ErrorMessageList[errorCode])
    {
        if (string.IsNullOrEmpty(Message))
            Message = ErrorMessages.ErrorMessageList["PBI999"];
    }
}
