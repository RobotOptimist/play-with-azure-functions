using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctionRestAPI.Clients;

namespace AzureFunctionRestAPI
{
    public class Function1
    {
        private readonly NothingClient nothingClient;

        public Function1(NothingClient nothingClient)
        {
            this.nothingClient = nothingClient;
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string nothingMessage = nothingClient.DoNothing();

            string responseMessage = string.IsNullOrEmpty(name)
                ? $"This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response. {nothingMessage}"
                : $"Hello, {name}. This HTTP triggered function executed successfully. {nothingMessage}";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("Function2")]
        public async Task<IActionResult> RunAgain(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
