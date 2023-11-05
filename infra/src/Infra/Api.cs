using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Infra;

public class Api : RestApi
{
    public Api(
        Construct scope,
        string id,
        RestApiProps props) : base(
        scope,
        id,
        props)
    {
    }
   
    public Api WithEndpoint(string path, HttpMethod method, Function function)
    {
        IResource? lastResource = null;

        foreach (var pathSegment in path.Split('/'))
        {
            var sanitisedPathSegment = pathSegment.Replace(
                "/",
                "");

            if (string.IsNullOrEmpty(sanitisedPathSegment))
            {
                continue;
            }

            if (lastResource == null)
            {
                lastResource = this.Root.GetResource(sanitisedPathSegment) ?? this.Root.AddResource(sanitisedPathSegment);
                continue;
            }

            lastResource = lastResource.GetResource(sanitisedPathSegment) ??
                           lastResource.AddResource(sanitisedPathSegment);
        }

        lastResource?.AddMethod(
            method.ToString().ToUpper(),
            new LambdaIntegration(function),
            new MethodOptions
            {
                MethodResponses = new IMethodResponse[]
                {
                    new MethodResponse { StatusCode = "200" },
                    new MethodResponse { StatusCode = "400" },
                    new MethodResponse { StatusCode = "500" }
                },
            });

        return this;
    }
    
    public Api WithEndpoint(string path, Function function)
    {
        IResource? lastResource = null;

        foreach (var pathSegment in path.Split('/'))
        {
            var sanitisedPathSegment = pathSegment.Replace(
                "/",
                "");

            if (string.IsNullOrEmpty(sanitisedPathSegment))
            {
                continue;
            }

            if (lastResource == null)
            {
                lastResource = this.Root.GetResource(sanitisedPathSegment) ?? this.Root.AddResource(sanitisedPathSegment);
                continue;
            }

            lastResource = lastResource.GetResource(sanitisedPathSegment) ??
                           lastResource.AddResource(sanitisedPathSegment);
        }

        lastResource?.AddMethod(
            "ANY",
            new LambdaIntegration(function),
            new MethodOptions
            {
                MethodResponses = new IMethodResponse[]
                {
                    new MethodResponse { StatusCode = "200" },
                    new MethodResponse { StatusCode = "400" },
                    new MethodResponse { StatusCode = "500" }
                },
            });

        return this;
    }
}