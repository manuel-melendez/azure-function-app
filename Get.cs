using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace My.Function
{
    public class Get
    {
        private readonly ILogger<Get> _logger;
        private readonly TableClient _tableClient;

        public Get(ILogger<Get> logger, TableClient tableClient)
        {
            _logger = logger;
            _tableClient = tableClient;
        }

        [Function("Get")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req
        )
        {
            try
            {
                List<TableEntity> entities = new List<TableEntity>();
                await foreach (var entity in _tableClient.QueryAsync<TableEntity>())
                {
                    entities.Add(entity);
                }

                _logger.LogInformation("Entities retrieved successfully");
                return new OkObjectResult(entities);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving entities");
                return new BadRequestObjectResult("Error retrieving entities");
            }
        }
    }
}
