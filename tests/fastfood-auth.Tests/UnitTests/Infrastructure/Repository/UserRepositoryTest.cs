using Amazon.CognitoIdentityProvider.Model;
using Amazon.DynamoDBv2.Model;
using fastfood_auth.Domain.Entity;
using fastfood_auth.Infra.Cognito.Creation;
using fastfood_auth.Infra.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.UnitTests.Infrastructure.Repository;

public class UserRepositoryTest : TestFixture
{
    [Test, Description("Should add user successfully")]
    public async Task ShouldAddUserAsync()
    {
        var entity = _modelFakerFactory.GenerateRequest<UserEntity>();

        _dynamoMock.SetupPutItemAsync(new PutItemResponse() { HttpStatusCode = System.Net.HttpStatusCode.OK, ContentLength = 1234567 });

        UserRepository service = new(_dynamoMock.Object);

        var result = await service.AddUserAsync(entity, default);

        Assert.True(result);

        _dynamoMock.VerifyPutItemAsync();
        _dynamoMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyNoOtherCalls();
        _userAuthenticationMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should get user by cpf successfully")]
    public async Task ShouldGetUserByCPFOrEmailAsync()
    {
        var entity = _modelFakerFactory.GenerateRequest<UserEntity>();

        _dynamoMock.SetupScanAsync(new ScanResponse()
        {
            Items = [
                _modelFakerFactory.GenerateRequest<Dictionary<string, AttributeValue>>()
            ],
            HttpStatusCode = System.Net.HttpStatusCode.OK
        });

        UserRepository service = new(_dynamoMock.Object);

        var result = await service.GetUserByCPFOrEmailAsync(entity.Identification, entity.Email, default);

        Assert.That(result, Is.Not.Null);

        _dynamoMock.VerifyScanAsync();
        _dynamoMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyNoOtherCalls();
        _userAuthenticationMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should get user successfully")]
    public async Task ShouldGetUsersAsync()
    {
        _dynamoMock.SetupScanAsync(new ScanResponse()
        {
            Items = [
                _modelFakerFactory.GenerateRequest<Dictionary<string, AttributeValue>>()
            ],
            HttpStatusCode = System.Net.HttpStatusCode.OK
        });

        UserRepository service = new(_dynamoMock.Object);

        var result = await service.GetUsersAsync(default);

        Assert.That(result, Is.Not.Null);

        _dynamoMock.VerifyScanAsync();
        _dynamoMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyNoOtherCalls();
        _userAuthenticationMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }

    [Test, Description("Should delete user successfully")]
    public async Task ShouldDeleteUsersAsync()
    {
        var entity = _modelFakerFactory.GenerateRequest<UserEntity>();

        _dynamoMock.SetupDeleteItemAsync(new DeleteItemResponse() { HttpStatusCode = System.Net.HttpStatusCode.OK, ContentLength = 1234567 });

        UserRepository service = new(_dynamoMock.Object);

        var task = service.DeleteUserAsync(entity.Identification, entity.Email, default);
        task.Wait();

        Assert.That(task.IsCompletedSuccessfully, Is.True);

        _dynamoMock.VerifyDeleteItemAsync();
        _dynamoMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyNoOtherCalls();
        _userAuthenticationMock.VerifyNoOtherCalls();
        _userCreationMock.VerifyNoOtherCalls();
    }
}