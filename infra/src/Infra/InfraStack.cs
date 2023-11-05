using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using Infrastructure.Definitions;
using XaasKit.CDK.AWS.Lambda.DotNet;

namespace Infra
{
    public class InfraStack : Stack
    {
        internal InfraStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var table = new Table(this, "DatabaseTable", new DatabaseProps().TableProps);

            var dotnetLambda = new DotNetFunction(this, "ApiEndpoint", new DotNetFunctionProps
            {
                ProjectDir = "../src/ProductsAPI/",
                Handler = "ProductsAPI",
                Runtime = Runtime.DOTNET_6,
                Architecture = Architecture.X86_64,
                Environment = new Dictionary<string, string>(1)
                {
                    { "TABLE_NAME", table.TableName }
                },
                FunctionName = "ProductsApiEndpoint",
                MemorySize = 1024,
                Timeout = Duration.Seconds(29),
            });

            table.GrantReadWriteData(dotnetLambda);

            var api = new Api(this, "ProductAPI", new RestApiProps())
                .WithEndpoint("/{proxy+}", dotnetLambda);
        }
    }
}
