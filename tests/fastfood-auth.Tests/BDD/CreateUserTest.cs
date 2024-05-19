using fastfood_auth.API.Controllers;
using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Application.UseCases.CreateUser;
using fastfood_auth.Domain.Enum;
using fastfood_auth.Tests.UnitTests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using TechTalk.SpecFlow;

namespace fastfood_auth.Tests.BDD;

[TestFixture]
public class CreateUserTest : TestFixture
{
    private Mock<IMediator> _mediatorMock;
    private CreateUserRequest _request;
    private IActionResult _result;

    [Test, Description("")]
    public async Task CreateANewOrder()
    {
        GivenIHaveAValidCreateUserRequest();
        GivenTheRepositoryReturnsASuccessfulResult();
        await WhenIRequestAUserCreation();
        ThenTheResultShouldBeACreatedResult();
    }

    [Given(@"I have a valid create user request")]
    public void GivenIHaveAValidCreateUserRequest()
    {
        _request = new CreateUserRequest("Marcello", "email@email.com", "12345678909");
    }

    [Given(@"the repository returns a successful result")]
    public void GivenTheRepositoryReturnsASuccessfulResult()
    {
        _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateUserRequest>(), default))
            .ReturnsAsync(Result<CreateUserResponse>.Success(new CreateUserResponse(){}, StatusResponse.CREATED));
    }

    [When(@"I request a user creation")]
    public async Task WhenIRequestAUserCreation()
    {
        UserController controller = new UserController(_mediatorMock.Object);

        _result = await controller.CreateUser(_request, default);
    }

    [Then(@"the result should be a CreatedResult")]
    public void ThenTheResultShouldBeACreatedResult()
    {
        ObjectResult? objectResult = _result as ObjectResult;
        Assert.That(objectResult, Is.Not.Null);
        Assert.That(objectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        Response<object>? response = objectResult.Value as Response<object>;
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Status, Is.EqualTo(nameof(StatusResponse.CREATED)));
    }
}
