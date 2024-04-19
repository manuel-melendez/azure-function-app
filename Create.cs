using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace My.Function
{
    public class Create
    {
        private readonly ILogger<Create> _logger;
        private readonly TableClient _tableClient;

        public Create(ILogger<Create> logger, TableClient tableClient)
        {
            _logger = logger;
            _tableClient = tableClient;
        }

        [Function("Create")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req
        )
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            MyEntity? data = JsonConvert.DeserializeObject<MyEntity>(requestBody);

            if (data == null)
            {
                return new BadRequestObjectResult("Invalid request body");
            }
            data.PartitionKey = "MypartitionKey";
            data.RowKey = Guid.NewGuid().ToString();

            try
            {
                _tableClient.AddEntity(
                    new TableEntity(data.PartitionKey, data.RowKey)
                    {
                        { "Subject", data.Subject },
                        { "Body", data.Body },
                        { "BlobUrl", data.BlobUrl },
                        { "Type", data.Type }
                    }
                );
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                return new OkObjectResult("successfully created entity");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating entity");
                return new BadRequestObjectResult("Error creating entity");
            }
        }
    }
}
