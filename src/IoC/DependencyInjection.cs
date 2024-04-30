using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using fastfood_auth.Data.Repository;
using fastfood_auth.Interface;
using fastfood_auth.Services;
using NLog;
using NLog.AWS.Logger;
using NLog.Config;

namespace fastfood_auth.IoC;

public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureCognito(services);
        ConfigureLogging(services);
        ConfigureServices(services);
        ConfigureDatabase(services);
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        _ = services.AddTransient<IUserService, UserService>();
        _ = services.AddSingleton<ICognitoService, CognitoService>();
        _ = services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void ConfigureCognito(IServiceCollection services)
    {
        string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_DYNAMO");
        string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY_DYNAMO");

        AWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);

        AmazonCognitoIdentityProviderClient cognitoProvider = new AmazonCognitoIdentityProviderClient(credentials, Amazon.RegionEndpoint.USEast1);

        _ = services.AddSingleton(cognitoProvider);
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
