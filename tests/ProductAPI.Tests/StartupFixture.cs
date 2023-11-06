using Amazon.DynamoDBv2;
using DotNet.Testcontainers.Containers;
using Infrastructure.Definitions;
using Testcontainers.DynamoDb;
using Testcontainers.LocalStack;

namespace ProductAPI.Tests;

public sealed class StartupFixture : IDisposable
{
    private readonly IContainer container;

    public AmazonDynamoDBClient DynamoDbClient;
    
    public StartupFixture()
    {
        var databaseDefinition = new DatabaseProps();
        
        container = new DynamoDbBuilder()
            .WithPortBinding(8000, true)
            .Build();

        var serviceUrl = "";

        if (Environment.GetEnvironmentVariable("USE_LOCAL_STACK") == "Y")
        {
            var localStackContainer = new LocalStackBuilder()
                .Build();
            
            localStackContainer.StartAsync().GetAwaiter().GetResult();

            serviceUrl = localStackContainer.GetConnectionString();
        }
        else
        {
            container.StartAsync().GetAwaiter().GetResult();

            serviceUrl = $"http://localhost:{container.GetMappedPublicPort(8000)}";
        }
        
        container.StartAsync().GetAwaiter().GetResult();

        this.DynamoDbClient = new AmazonDynamoDBClient(new AmazonDynamoDBConfig()
        {
            ServiceURL = serviceUrl
        });

        this.DynamoDbClient.CreateTableAsync(databaseDefinition.AsCreateRequest()).GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        container.StopAsync();
    }
}