using Amazon.DynamoDBv2;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Infrastructure.Definitions;

namespace ProductAPI.Tests;

public class StartupFixture : IDisposable
{
    private readonly IContainer container;

    public AmazonDynamoDBClient DynamoDbClient;
    
    public StartupFixture()
    {
        container = new ContainerBuilder()
            // Set the image for the container to "testcontainers/helloworld:1.1.0".
            .WithImage("amazon/dynamodb-local:1.24.0")
            // Bind port 8080 of the container to a random port on the host.
            .WithPortBinding(8000, 8000)
            .Build();

        container.StartAsync().GetAwaiter().GetResult();
        
        this.DynamoDbClient = new AmazonDynamoDBClient(new AmazonDynamoDBConfig()
        {
            ServiceURL = "http://localhost:8000"
        });
        
        var databaseDefinition = new DatabaseProps();

        this.DynamoDbClient.CreateTableAsync(databaseDefinition.AsCreateRequest()).GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        container.StopAsync();
    }
}