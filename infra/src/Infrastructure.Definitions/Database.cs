using System.Data;
using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Constructs;
using Attribute = Amazon.CDK.AWS.DynamoDB.Attribute;

namespace Infrastructure.Definitions;

public class DatabaseProps
{
    public TableProps TableProps;
    
    public DatabaseProps()
    {
        this.TableProps = new TableProps()
        {
            TableName = "Products",
            BillingMode = BillingMode.PAY_PER_REQUEST,
            RemovalPolicy = RemovalPolicy.DESTROY,
            PartitionKey = new Attribute
            {
                Name = "PK",
                Type = AttributeType.STRING
            },
            SortKey = new Attribute()
            {
                Name = "SK",
                Type = AttributeType.STRING
            }
    };
    }
}