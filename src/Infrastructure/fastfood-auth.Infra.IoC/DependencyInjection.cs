using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Domain.Contracts.Repository;
using fastfood_auth.Infra.Cognito.Authentication;
using fastfood_auth.Infra.Cognito.Creation;
using fastfood_auth.Infra.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.AWS.Logger;
using NLog.Config;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace fastfood_auth.Infra.IoC;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureCognito(services);
        ConfigureLogging(services);
        services.ConfigureServices();
        services.ConfigureAutomapper();
        services.ConfigureMediatr();
        ConfigureDatabase(services);
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void ConfigureCognito(IServiceCollection services)
    {
        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_DYNAMO");
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY_DYNAMO");

        AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);

        AmazonCognitoIdentityProviderClient cognitoProvider = new AmazonCognitoIdentityProviderClient(credentials, Amazon.RegionEndpoint.USEast1);

        services.AddSingleton(cognitoProvider);
        services.AddSingleton<IUserCreation, CognitoUserCreation>();
        services.AddSingleton<IUserAuthentication, CognitoUserAuthentication>();
    }

    private static void ConfigureLogging(IServiceCollection services)
    {
        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_DYNAMO");
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY_DYNAMO");

        AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);

        LoggingConfiguration config = new LoggingConfiguration();

        config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, new AWSTarget()
        {
            LogGroup = Environment.GetEnvironmentVariable("LOG_GROUP"),
            Region = Environment.GetEnvironmentVariable("LOG_REGION"),
            Credentials = credentials
        });

        LogManager.Configuration = config;

        Logger log = LogManager.GetCurrentClassLogger();

        _ = services.AddSingleton(log);
    }
    private static void ConfigureAutomapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Result).Assembly);
    }

    private static void ConfigureMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Result).Assembly));
    }

    private static void ConfigureDatabase(IServiceCollection services)
    {
        AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig
        {
            RegionEndpoint = Amazon.RegionEndpoint.USEast1
        };
        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_DYNAMO");
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY_DYNAMO");

        AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);

        _ = services.AddSingleton<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(credentials, clientConfig));
    }
}
