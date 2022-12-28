using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace TaskBoard.Http
{
    public class HealthFunctions
    {
        [FunctionName("health")]
        public IActionResult Health([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest request, ILogger log)
        {
            return new OkResult();
        }
    }
}