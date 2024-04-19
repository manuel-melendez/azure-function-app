using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace My.Function
{
    public class Edit
    {
        private readonly ILogger<Edit> _logger;
        private readonly TableClient _tableClient;

        public Edit(ILogger<Edit> logger, TableClient tableClient)
        {
            _logger = logger;
            _tableClient = tableClient;
        }

        [Function("Edit")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "put",
                Route = "Edit/{partitionKey}/{rowKey}"
            )]
                HttpRequest req,
            string partitionKey,
            string rowKey
        )
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<TableEntity>(partitionKey, rowKey);
                var entity = response.Value;

                string request = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<MyEntity>(request);

                if (data != null)
                {
                    entity["Subject"] = data.Subject;
                    entity["Body"] = data.Body;
                    entity["BlobUrl"] = data.BlobUrl;
                    entity["Type"] = data.Type;
                }

                await _tableClient.UpdateEntityAsync(entity, entity.ETag);

                _logger.LogInformation("Entity updated successfully");
                return new OkObjectResult(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating entity");
                return new BadRequestObjectResult("Error updating entity");
            }
        }
    }
}
