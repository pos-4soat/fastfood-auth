using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Tests.UnitTests;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.Mocks;

public class MemoryCacheMock : BaseCustomMock<IMemoryCache>
{
    public MemoryCacheMock(TestFixture testFixture) : base(testFixture)
    {
    }

    public void SetupTryGetValue(object expectedReturn)
        => Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedReturn))
            .Returns(expectedReturn != null);

    public void SetupSet()
        => Setup(x => x.Set(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<TimeSpan>()))
            .Returns(null);
}