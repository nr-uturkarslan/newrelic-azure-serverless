using ArchiveService.Azure.BlobContainer;
using ArchiveService.Azure.ServiceBus;
using ArchiveService.Commons.Constants;
using ArchiveService.Services.Create;

var builder = WebApplication.CreateBuilder(args);

// Get environment variables.
GetEnvironmentVariables();

// Add services to the container.
builder.Services.AddSingleton<IBlobHandler, BlobHandler>();
builder.Services.AddHostedService<ServiceBusHandler>();
builder.Services.AddSingleton<IListFileService, ListFileService>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run("http://*:8080");

void GetEnvironmentVariables()
{
    Console.WriteLine("Getting environment variables...");

    var storageAccountName = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_NAME");
    if (string.IsNullOrEmpty(storageAccountName))
    {
        Console.WriteLine("[STORAGE_ACCOUNT_NAME] is not provided");
        Environment.Exit(1);
    }
    EnvironmentVariables.STORAGE_ACCOUNT_NAME = storageAccountName;

    var blobContainerName = Environment.GetEnvironmentVariable("BLOB_CONTAINER_NAME");
    if (string.IsNullOrEmpty(blobContainerName))
    {
        Console.WriteLine("[BLOB_CONTAINER_NAME] is not provided");
        Environment.Exit(1);
    }
    EnvironmentVariables.BLOB_CONTAINER_NAME = blobContainerName;

    var serviceBusFqdn = Environment.GetEnvironmentVariable("SERVICE_BUS_FQDN");
    if (string.IsNullOrEmpty(serviceBusFqdn))
    {
        Console.WriteLine("[SERVICE_BUS_FQDN] is not provided");
        Environment.Exit(1);
    }
    EnvironmentVariables.SERVICE_BUS_FQDN = serviceBusFqdn;

    var serviceBusQueueName = Environment.GetEnvironmentVariable("SERVICE_BUS_QUEUE_NAME");
    if (string.IsNullOrEmpty(serviceBusQueueName))
    {
        Console.WriteLine("[SERVICE_BUS_QUEUE_NAME] is not provided");
        Environment.Exit(1);
    }
    EnvironmentVariables.SERVICE_BUS_QUEUE_NAME = serviceBusQueueName;
}

