using fastfood_auth.Application.UseCases.CreateUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.UnitTests.Application.CreateUser;

internal class CreateUserValidatorTest : TestFixture
{
    private CreateUserValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateUserValidator();
    }

    [Test]
    public void ShouldValidateNameRequirement()
    {
        CreateUserRequest request = _modelFakerFactory.GenerateRequestWith<CreateUserRequest, string>(r => r.Name, null);

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE002");
    }

    [Test]
    public void ShouldValidateNameLenght()
    {
        CreateUserRequest request = _modelFakerFactory.GenerateRequestWith<CreateUserRequest, string>(r => r.Name, "as");

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE003");
    }

    [Test]
    public void ShouldValidateEmailRequirement()
    {
        CreateUserRequest request = _modelFakerFactory.GenerateRequestWith<CreateUserRequest, string>(r => r.Email, null);

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE004");
    }

    [Test]
    public void ShouldValidateEmailFormat()
    {
        CreateUserRequest request = _modelFakerFactory.GenerateRequestWith<CreateUserRequest, string>(r => r.Email, "as");

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE005");
    }

    [Test]
    public void ShouldValidateIdentificationRequirement()
    {
        CreateUserRequest request = _modelFakerFactory.GenerateRequestWith<CreateUserRequest>(
            (r => r.Email, "email@email.com"),
            (r => r.Identification, null)
        );

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE006");
    }

    [Test]
    public void ShouldValidateIdentificationFormat()
    {
        CreateUserRequest request = _modelFakerFactory.GenerateRequestWith<CreateUserRequest>(
            (r => r.Email, "email@email.com"),
            (r => r.Identification, "as")
        );

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE007");
    }
}