using fastfood_auth.Handlers;
using fastfood_auth.IoC;
using fastfood_auth.Middleware;
using fastfood_auth.Utils;
using fastfood_auth.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.SwaggerGen;
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

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddMemoryCache();

services.RegisterServices(configuration);

services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
services.AddFluentValidationRulesToSwagger();

services
    .AddControllers(o =>
    {
        _ = o.Filters.Add(typeof(ValidationAttributeHandler));
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
    .Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(setup =>
    {
        setup.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Fast Food Authentication",
                Version = "v1",
                Description = "Documentacao para o webservice, explicando e detalhando cada endpoint"
            });

        setup.EnableAnnotations();
        setup.IgnoreObsoleteActions();
        setup.IgnoreObsoleteProperties();

        setup.DocInclusionPredicate((version, desc) =>
        {
            if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
            IEnumerable<ApiVersion> versions = methodInfo.DeclaringType
                .GetCustomAttributes(true)
                .OfType<ApiVersionAttribute>()
                .SelectMany(attr => attr.Versions);
            return versions.Any(v => $"v{v}" == version);
        });

        string filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
        setup.IncludeXmlComments(filePath);
    })
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
    });

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHealthChecks("/health", new HealthCheckOptions { ResponseWriter = HealthCheckResponseWriter.Write });

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();

    _ = endpoints.MapDefaultControllerRoute();
    _ = endpoints.MapHealthChecks("health").WithMetadata(new AllowAnonymousAttribute());
    _ = endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
    {
        Predicate = (_) => false
    }).WithMetadata(new AllowAnonymousAttribute());
    _ = endpoints.MapHealthChecks("/metrics", new HealthCheckOptions
    {
        Predicate = (_) => false
    }).WithMetadata(new AllowAnonymousAttribute());
    _ = endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = (check) => check.Tags.Contains("ready"),
        ResponseWriter = HealthCheckResponseWriter.Write
    }).WithMetadata(new AllowAnonymousAttribute());
});

app.Run();