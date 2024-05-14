using fastfood_auth.API.HealthCheck;
using fastfood_auth.API.Middleware;
using fastfood_auth.Application.Shared.Behavior;
using fastfood_auth.Infra.IoC;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;
using System.Text.Json;
using System.Text.Unicode;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;
ILoggingBuilder logging = builder.Logging;

configuration
    .AddUserSecrets<Program>(optional: true)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

logging.ClearProviders();
logging.AddConsole();

services
    .AddHealthChecks()
    .AddCheck<SimpleHealthCheck>("live", failureStatus: HealthStatus.Unhealthy, tags: new[] { "live" });

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddMemoryCache();

services
    .AddControllers(o =>
    {
        o.Filters.Add(typeof(ValidationBehavior));
        o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All);
    });

services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(setup =>
    {
        setup.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "FastFood Auth",
                Version = "v1"
            });

        string filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
        setup.IncludeXmlComments(filePath);
    });

services.RegisterServices(configuration);

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();