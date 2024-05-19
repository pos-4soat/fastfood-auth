using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Domain.Enum;
using fastfood_auth.Tests.UnitTests;
using MediatR;
using Moq;

namespace fastfood_auth.Tests.Mocks;

public class MediatorMock<TRequest, TResponse>(TestFixture testFixture) : BaseCustomMock<IMediator>(testFixture) where TRequest : notnull
{
    public void SetupSendAsync(TResponse response, StatusResponse statusResponse)
        => Setup(x => x.Send(It.IsAny<TRequest>(), default))
            .ReturnsAsync(Result<TResponse>.Success(response, statusResponse));
}

