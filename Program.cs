using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        string storageUri = Environment.GetEnvironmentVariable("StorageUri");
        string storageAccountName = Environment.GetEnvironmentVariable("StorageAccountName");
        string storageAccountKey = Environment.GetEnvironmentVariable("StorageAccountKey");
        var tableClient = new TableClient(
            new Uri(storageUri),
            "ManuelTable",
            new TableSharedKeyCredential(storageAccountName, storageAccountKey)
        );

        services.AddSingleton(tableClient);
    })
    .Build();

host.Run();
