using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Infrastructure.Definitions;
using BillingMode = Amazon.CDK.AWS.DynamoDB.BillingMode;

namespace ProductAPI.Tests;

public static class CdkExtensions
{
    public static CreateTableRequest AsCreateRequest(this DatabaseProps props)
    {
        var keySchema = new List<KeySchemaElement>();
        keySchema.Add(new KeySchemaElement(props.TableProps.PartitionKey.Name, KeyType.HASH));

        var attributeDefinitions = new List<AttributeDefinition>();
        attributeDefinitions.Add(new AttributeDefinition(props.TableProps.PartitionKey.Name, ScalarAttributeType.S));
        
        if (props.TableProps.SortKey != null)
        {
            keySchema.Add(new KeySchemaElement(props.TableProps.SortKey.Name, KeyType.HASH));
            attributeDefinitions.Add(new AttributeDefinition(props.TableProps.SortKey.Name, ScalarAttributeType.S));
        }

        var req = new CreateTableRequest(props.TableProps.TableName, keySchema);
        req.BillingMode = props.TableProps.BillingMode == BillingMode.PROVISIONED
            ? Amazon.DynamoDBv2.BillingMode.PROVISIONED
            : Amazon.DynamoDBv2.BillingMode.PAY_PER_REQUEST;

        req.AttributeDefinitions = attributeDefinitions;

        return req;
    }
}