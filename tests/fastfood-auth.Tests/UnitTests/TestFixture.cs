using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using AutoFixture;
using AutoMapper;
using Bogus;
using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Domain.Contracts.Repository;
using fastfood_auth.Tests.Mocks;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.AutoMock;

namespace fastfood_auth.Tests.UnitTests;

[TestFixture]
public abstract class TestFixture
{
    public Fixture AutoFixture { get; init; } = new();
    public Faker Faker { get; init; } = new();

    protected UserRepositoryMock _repositoryMock;
    protected UserAuthenticationMock _userAuthenticationMock;
    protected UserCreationMock _userCreationMock;
    protected MemoryCacheMock _memoryCacheMock;
    protected AmazonCognitoIdentityProviderClientMock _cognitoMock;
    protected AmazonDynamoDBMock _dynamoMock;
    protected IMapper _mapper;

    protected ModelFakerFactory _modelFakerFactory;
    protected AutoMocker _autoMocker;

    [OneTimeSetUp]
    public void GlobalPrepare()
    {
        _autoMocker = new();
        _modelFakerFactory = new(AutoFixture, Faker);
    }

    [SetUp]
    public async Task SetUpAsync()
    {
        AddCustomMocksToContainer();
        InstantiateCustomMocks();
        CreateMapper();
    }

    [TearDown]
    public void TearDown()
    {
        foreach (KeyValuePair<Type, object?> resolvedObject in _autoMocker.ResolvedObjects)
            (resolvedObject.Value as Mock)?.Invocations.Clear();
    }

    protected T CreateInstance<T>() where T : class
        => _autoMocker.CreateInstance<T>();

    protected TCustomMock GetCustomMock<TInterface, TCustomMock>() where TCustomMock : BaseCustomMock<TInterface> where TInterface : class
        => (_autoMocker.GetMock<TInterface>() as TCustomMock)!;

    protected Mock<T> GetMock<T>() where T : class
        => _autoMocker.GetMock<T>();

    #region Private Methods

    private void AddCustomMocksToContainer()
    {
        _autoMocker.Use(new UserCreationMock(this).ConvertToBaseType());
        _autoMocker.Use(new UserRepositoryMock(this).ConvertToBaseType());
        _autoMocker.Use(new UserAuthenticationMock(this).ConvertToBaseType());
        _autoMocker.Use(new MemoryCacheMock(this).ConvertToBaseType());
        _autoMocker.Use(new MemoryCacheMock(this).ConvertToBaseType());
        _autoMocker.Use(new AmazonDynamoDBMock(this).ConvertToBaseType());
    }

    private void InstantiateCustomMocks()
    {
        _repositoryMock = GetCustomMock<IUserRepository, UserRepositoryMock>();
        _userCreationMock = GetCustomMock<IUserCreation, UserCreationMock>();
        _userAuthenticationMock = GetCustomMock<IUserAuthentication, UserAuthenticationMock>();
        _memoryCacheMock = GetCustomMock<IMemoryCache, MemoryCacheMock>();
        _dynamoMock = GetCustomMock<IAmazonDynamoDB, AmazonDynamoDBMock>();
        _cognitoMock = new AmazonCognitoIdentityProviderClientMock();
    }

    private void CreateMapper()
    {
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Result).Assembly));

        _mapper = config.CreateMapper();
    }
    #endregion Private Methods
}
