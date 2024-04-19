using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace My.Function
{
    public class Delete
    {
        private readonly ILogger<Delete> _logger;
        private readonly TableClient _tableClient;

        public Delete(ILogger<Delete> logger, TableClient tableClient)
        {
            _logger = logger;
            _tableClient = tableClient;
        }

        [Function("Delete")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "delete",
                Route = "Delete/{partitionKey}/{rowKey}"
            )]
                HttpRequest req,
            string partitionKey,
            string rowKey
        )
        {
            try
            {
                await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
                _logger.LogInformation("Entity deleted successfully");
                return new OkObjectResult("Entity deleted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting entity");
                return new BadRequestObjectResult("Error deleting entity");
            }
        }
    }
}
