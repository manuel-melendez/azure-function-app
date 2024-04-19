using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace My.Function
{
    public class GetById
    {
        private readonly ILogger<GetById> _logger;
        private readonly TableClient _tableClient;

        public GetById(ILogger<GetById> logger, TableClient tableClient)
        {
            _logger = logger;
            _tableClient = tableClient;
        }

        [Function("GetById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "GetById/{partitionKey}/{rowKey}"
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
                _logger.LogInformation("Entity retrieved successfully");
                return new OkObjectResult(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving entity");
                return new BadRequestObjectResult("Error retrieving entity");
            }
        }
    }
}
