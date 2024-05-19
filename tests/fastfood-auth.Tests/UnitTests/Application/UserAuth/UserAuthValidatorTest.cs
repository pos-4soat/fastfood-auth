using fastfood_auth.Application.UseCases.CreateUser;
using fastfood_auth.Application.UseCases.UserAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.UnitTests.Application.UserAuth;

public class UserAuthValidatorTest : TestFixture
{
    private UserAuthValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new UserAuthValidator();
    }

    [Test]
    public void ShouldValidateCpfRequirement()
    {
        UserAuthRequest request = _modelFakerFactory.GenerateRequestWith<UserAuthRequest, string>(r => r.cpf, null);

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE006");
    }

    [Test]
    public void ShouldValidateCpfValid()
    {
        UserAuthRequest request = _modelFakerFactory.GenerateRequestWith<UserAuthRequest, string>(r => r.cpf, "123456");

        FluentValidation.Results.ValidationResult result = _validator.Validate(request);

        AssertExtensions.AssertValidation(result, "ABE007");
    }
}
