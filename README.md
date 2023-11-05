# Serverless Test Containers

A sample repository testing an integration between:

- AWS CDK to define serverless applications with .NET
- A CRUD API build with ASP.NET for a web application
- xUnit along with TestContainers to test with emulation

[Test Containers](https://testcontainers.com) is an open source framework for providing throwaway, lightweight instances of databases, message brokers, web browsers, or just about anything that can run in a Docker container.

In this example, the CDK code is defined under the `/infra` folder. A separate library named `Infrastructure.Definitions` defines the `TableProps` for the DynamoDB `Table` resource.

The unit tests, defined under `tests/ProductAPI.Tests` reference the `Infrastructure.Definitions` project and use the properties to execute a `CreateTableRequest` against an emulated DynamoDB table using the DynamoDBLocal project. The DynamoDBLocal container is started up using TestContainers.

The actual application code, defined under `/src/ProductsAPI` also references the `Infrastructure.Definitions` library to ensure the `ProductRepository` uses the correct table names for the partition key.

Combining all these references, means a breaking change to the `Table` definition in the CDK code would fail the unit tests.

To see this in action, first run the below command which will run the DynamoDBLocal container and run unit tests. Ensure you have Docker running first.

```
dotnet test tests/ProductAPI.Tests/
```

Now, open up the [Database definition](/infra/src/Infrastructure.Definitions/Database.cs) and update the PartitionKey property from `id` to be `broken`.

Now re-run the tests:

```
dotnet test tests/ProductAPI.Tests/
```

And they will fail. Updating your CDK definition to not match what your application code expects fails the unit tests.
