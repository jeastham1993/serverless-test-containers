using System.Net;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ProductsAPI.DataAccess;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(new ProductRepository(new AmazonDynamoDBClient()));

builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

var app = builder.Build();

var productRepo = app.Services.GetRequiredService<ProductRepository>();

app.MapGet("/{id}", async (HttpContext context) =>
{
    var id = context.Request.RouteValues["id"].ToString();
    
    var product = await productRepo.GetProduct(id);

    context.Response.StatusCode = (int) HttpStatusCode.OK;
    await context.Response.WriteAsJsonAsync(product);
});

app.Run();