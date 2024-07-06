using fastfood_auth.Application.UseCases.CreateUser;
using fastfood_auth.Application.UseCases.DeleteUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.UnitTests.Application.DeleteUser;

public class DeleteUserValidatorTest : TestFixture
{
    private DeleteUserValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteUserValidator();
    }

    [Test]
    public void ShouldValidateNameRequirement()
    {
        DeleteUserRequest request = _modelFakerFactory.GenerateRequestWith<DeleteUserRequest, string>(r => r.Name, null);

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE002");
    }

    [Test]
    public void ShouldValidateNameLenght()
    {
        DeleteUserRequest request = _modelFakerFactory.GenerateRequestWith<DeleteUserRequest, string>(r => r.Name, "as");

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE003");
    }

    [Test]
    public void ShouldValidateEmailRequirement()
    {
        DeleteUserRequest request = _modelFakerFactory.GenerateRequestWith<DeleteUserRequest, string>(r => r.Email, null);

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE004");
    }

    [Test]
    public void ShouldValidateEmailFormat()
    {
        DeleteUserRequest request = _modelFakerFactory.GenerateRequestWith<DeleteUserRequest, string>(r => r.Email, "as");

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE005");
    }

    [Test]
    public void ShouldValidateIdentificationRequirement()
    {
        DeleteUserRequest request = _modelFakerFactory.GenerateRequestWith<DeleteUserRequest>(
            (r => r.Email, "email@email.com"),
            (r => r.Identification, null)
        );

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE006");
    }

    [Test]
    public void ShouldValidateIdentificationFormat()
    {
        DeleteUserRequest request = _modelFakerFactory.GenerateRequestWith<DeleteUserRequest>(
            (r => r.Email, "email@email.com"),
            (r => r.Identification, "as")
        );

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE007");
    }
}
