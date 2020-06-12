using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace GetCommand
{
    public static class GetAllCommands
    {
        [FunctionName("GetAllCommands")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            [DocumentDB(
                databaseName: "ToDoList",
                collectionName: "Items",
                ConnectionStringSetting = "cosmosdbcon",
                SqlQuery = "SELECT * FROM c order by c.id ")] IEnumerable<Command> toDoItem,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            if (toDoItem == null)
            {
                log.Info($"ToDo item not found");
                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                foreach(Command c in toDoItem)
                {
                    log.Info($"Found ToDo item, Description={c.HowTo},Id={c.id}");
                }
                
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            { Content = new StringContent(JsonConvert.SerializeObject(toDoItem, Formatting.Indented), Encoding.UTF8, "application/json") };



        }
    }
    public class Command
    {
        public string id { get; set; }

        
        public string HowTo { get; set; }
        
        public string Line { get; set; }
        
        public string Platform { get; set; }
    }
}
