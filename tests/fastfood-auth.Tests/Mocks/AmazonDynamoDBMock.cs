using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Tests.UnitTests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Tests.Mocks;

public class AmazonDynamoDBMock : BaseCustomMock<IAmazonDynamoDB>
{
    public AmazonDynamoDBMock(TestFixture testFixture) : base(testFixture)
    {
    }

    public void SetupPutItemAsync(PutItemResponse expectedReturn)
        => Setup(x => x.PutItemAsync(It.IsAny<PutItemRequest>(), default))
            .ReturnsAsync(expectedReturn);

    public void SetupScanAsync(ScanResponse expectedReturn)
        => Setup(x => x.ScanAsync(It.IsAny<ScanRequest>(), default))
            .ReturnsAsync(expectedReturn);

    public void VerifyPutItemAsync(Times? times = null)
        => Verify(x => x.PutItemAsync(It.IsAny<PutItemRequest>(), default), times ?? Times.Once());

    public void VerifyScanAsync(Times? times = null)
        => Verify(x => x.ScanAsync(It.IsAny<ScanRequest>(), default), times ?? Times.Once());
}