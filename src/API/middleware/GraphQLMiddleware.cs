using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.middleware
{
    public class GraphQLRequest
    {
        private JObject _variables;
        
        public string OperationName { get; set; }
        public string Query { get; set; }

        public JObject Variables
        {
            get => _variables ?? JObject.FromObject(new { });
            set => _variables = value;
        }
    }

    public static class GraphQLMiddlewareExtension
    {
        public static void UseGraphQLMiddleware<TSchema>(this IApplicationBuilder appBuilder)
            where TSchema : ISchema
        {
            appBuilder.UseMiddleware<GraphQLMiddleware<TSchema>>();
        }
    }
    
    public class GraphQLMiddleware<TSchema> where TSchema : ISchema
    {
        private readonly RequestDelegate _next;
        private TSchema Schema { get; }

        private readonly IDocumentExecuter _executer;
        private readonly IDocumentWriter _writer;

        public GraphQLMiddleware(
            RequestDelegate next,
            TSchema schema,
            IDocumentExecuter executer,
            IDocumentWriter writer
        )
        {
            _next = next;
            Schema = schema;
            _executer = executer;
            _writer = writer;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (
                context.Request.Method.ToLower() != "post" &&
                context.Request.Path != "/graphql"
            )
            {
                await _next(context);
                return;
            }

            var request = Deserialize<GraphQLRequest>(context.Request.Body);

            var result = await _executer.ExecuteAsync(new ExecutionOptions
            {
                Schema = Schema,
                Query = request.Query,
                OperationName = request.OperationName,
                Inputs = request.Variables.ToInputs()
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) (result.Errors?.Any() == true ? HttpStatusCode.BadRequest : HttpStatusCode.OK);
            await context.Response.WriteAsync(_writer.Write(result));
        }

        private static T Deserialize<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return new JsonSerializer().Deserialize<T>(jsonReader);
                }
            }
        }
    }
}