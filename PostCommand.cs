using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace GetCommand
{
    public static class PostCommand
    {
        [FunctionName("PostCommand")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function,  "post", Route = null)]HttpRequestMessage req,
             [DocumentDB(
                databaseName: "ToDoList",
                collectionName: "Items",
                ConnectionStringSetting = "cosmosdbcon"
                )] ICollector<Command> commands,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();
            string HowTo = data?.HowTo;
            string Line = data?.Line;
            string PlatForm = data?.Platform;
            string id = data?.id;

            commands.Add(new Command()
            {
                HowTo = HowTo,
                Line = Line,
                Platform = PlatForm,
                id = id
            });


            return new HttpResponseMessage(HttpStatusCode.Created)
            { Content = new StringContent(JsonConvert.SerializeObject(commands, Formatting.Indented), Encoding.UTF8, "application/json") };
        }
    }
}
