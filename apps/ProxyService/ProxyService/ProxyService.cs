using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace ProxyService
{
    public class ProxyService
    {
        private readonly HttpClient _httpClient;

        public ProxyService(
            HttpClient httpClient
        )
        {
            _httpClient = httpClient;
        }

        [FunctionName("CreateDevice")]
        public async Task<IActionResult> CreateDevice(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "device/create")] HttpRequest req,
            ILogger logger)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = await _httpClient.GetAsync("https://www.google.com/");
            var str = await response.Content.ReadAsStringAsync();

            return new OkObjectResult(str);
        }

        //[FunctionName("Archive")]
        //public async Task<IActionResult> GetArchive(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "archive")] HttpRequest req,
        //    ILogger logger)
        //{
        //    logger.LogInformation("C# HTTP trigger function processed a request.");

        //    string name = req.Query["name"];

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    dynamic data = JsonConvert.DeserializeObject(requestBody);
        //    name = name ?? data?.name;

        //    string responseMessage = string.IsNullOrEmpty(name)
        //        ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
        //        : $"Hello, {name}. This HTTP triggered function executed successfully.";

        //    return new OkObjectResult(responseMessage);
        //}
    }
}

