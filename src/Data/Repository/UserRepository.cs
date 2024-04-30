using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using fastfood_auth.Data.Entity;
using fastfood_auth.Interface;
using System.Net;
using System.Text.Json;

namespace fastfood_auth.Data.Repository;

public class UserRepository(IAmazonDynamoDB dynamoDb) : IUserRepository
{
    public async Task<bool> AddUserAsync(UserEntity user, CancellationToken cancellationToken)
    {
        string userAsJson = JsonSerializer.Serialize(user);
        Document itemAsDocument = Document.FromJson(userAsJson);
        Dictionary<string, AttributeValue> itemAsAttribute = itemAsDocument.ToAttributeMap();

        PutItemRequest createItemRequest = new()
        {
            TableName = Environment.GetEnvironmentVariable("AWS_TABLE_NAME_DYNAMO"),
            Item = itemAsAttribute
        };

        PutItemResponse response = await dynamoDb.PutItemAsync(createItemRequest, cancellationToken);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<UserEntity?> GetUserByCPFOrEmailAsync(string identification, string email, CancellationToken cancellationToken)
    {
        ScanRequest request = new()
        {
            TableName = Environment.GetEnvironmentVariable("AWS_TABLE_NAME_DYNAMO"),
            FilterExpression = "identification = :identification OR email = :email",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":identification", new AttributeValue { S = identification } },
                { ":email", new AttributeValue { S = email } }
            }
        };

        ScanResponse response = await dynamoDb.ScanAsync(request, cancellationToken);

        if (response.Items.Count == 0)
            return null;

        Document itemAsDocument = Document.FromAttributeMap(response.Items.First());
        return JsonSerializer.Deserialize<UserEntity>(itemAsDocument.ToJson());
    }

    public async Task<IEnumerable<UserEntity>?> GetUsersAsync(CancellationToken cancellationToken)
    {
        ScanRequest request = new()
        {
            TableName = Environment.GetEnvironmentVariable("AWS_TABLE_NAME_DYNAMO")
        };

        ScanResponse response = await dynamoDb.ScanAsync(request, cancellationToken);
        if (response.Items.Count == 0)
            return null;

        IEnumerable<UserEntity> users = response.Items.Select(item =>
        {
            return new UserEntity(
                item.TryGetValue("identification", out AttributeValue? idValue) ? idValue.S : null,
                item.TryGetValue("name", out AttributeValue? nameValue) ? nameValue.S : null,
                item.TryGetValue("email", out AttributeValue? emailValue) ? emailValue.S : null,
                item.TryGetValue("clientid", out AttributeValue? clientValue) ? clientValue.S : null
            );
        });

        return users;
    }
}
