﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using fastfood_auth.Domain.Contracts.Repository;
using fastfood_auth.Domain.Entity;
using System.Net;
using System.Text.Json;

namespace fastfood_auth.Infra.Persistance;

public class UserRepository(IAmazonDynamoDB dynamoDb) : IUserRepository
{
    public async Task<bool> AddUserAsync(UserEntity user, CancellationToken cancellationToken)
    {
        string userAsJson = JsonSerializer.Serialize(user);
        Document itemAsDocument = Document.FromJson(userAsJson);
        Dictionary<string, AttributeValue> itemAsAttribute = itemAsDocument.ToAttributeMap();

        PutItemRequest createItemRequest = new PutItemRequest
        {
            TableName = Environment.GetEnvironmentVariable("AWS_TABLE_NAME_DYNAMO"),
            Item = itemAsAttribute
        };

        PutItemResponse response = await dynamoDb.PutItemAsync(createItemRequest, cancellationToken);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task DeleteUserAsync(string identification, string email, CancellationToken cancellationToken)
    {
        Dictionary<string, AttributeValue> key = new()
        {
            { "pk", new AttributeValue { S = identification } },
            { "sk", new AttributeValue { S = identification } }
        };

        DeleteItemRequest deleteItemRequest = new()
        {
            TableName = Environment.GetEnvironmentVariable("AWS_TABLE_NAME_DYNAMO"),
            Key = key
        };

        await dynamoDb.DeleteItemAsync(deleteItemRequest, cancellationToken);
    }

    public async Task<UserEntity> GetUserByCPFOrEmailAsync(string identification, string email, CancellationToken cancellationToken)
    {
        ScanRequest request = new ScanRequest
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

    public async Task<IEnumerable<UserEntity>> GetUsersAsync(CancellationToken cancellationToken)
    {
        ScanRequest request = new ScanRequest
        {
            TableName = Environment.GetEnvironmentVariable("AWS_TABLE_NAME_DYNAMO")
        };

        ScanResponse response = await dynamoDb.ScanAsync(request, cancellationToken);
        if (response.Items.Count == 0)
            return null;

        IEnumerable<UserEntity> users = response.Items.Select(item =>
        {
            return new UserEntity
            {
                Identification = item.ContainsKey("identification") ? item["identification"].S : null,
                Name = item.ContainsKey("name") ? item["name"].S : null,
                Email = item.ContainsKey("email") ? item["email"].S : null,
                CognitoUserIdentification = item.ContainsKey("clientid") ? item["clientid"].S : null
            };
        });

        return users;
    }
}
