using AutoFixture;
using Bogus;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using System.Linq.Expressions;
using System.Reflection;

namespace fastfood_auth.Tests.UnitTests;

public class ModelFakerFactory(Fixture autoFixture, Faker faker)
{
    private readonly Fixture _autoFixture = autoFixture;
    private readonly Faker _faker = faker;

    public TRequest GenerateRequest<TRequest>()
        => _autoFixture.Build<TRequest>()
            .Create();
    public TRequest GenerateRequestWith<TRequest, TProperty>(Expression<Func<TRequest, TProperty>> property, TProperty value)
        => _autoFixture.Build<TRequest>()
            .With(property, value)
            .Create();

    public TRequest GenerateRequestWith<TRequest>(params (Expression<Func<TRequest, object>> Property, object Value)[] properties)
    {
        var request = _autoFixture.Build<TRequest>();

        var firstProp = properties.FirstOrDefault();
        AutoFixture.Dsl.IPostprocessComposer<TRequest> withBuild = request.With(firstProp.Property, firstProp.Value);

        foreach (var prop in properties)
        {
            if (prop.Equals(properties.FirstOrDefault()))
                continue;

            withBuild = withBuild.With(prop.Property, prop.Value);
        }

        return withBuild.Create();
    }


    public IEnumerable<TRequest> GenerateManyRequest<TRequest>()
        => _autoFixture.CreateMany<TRequest>();
}
