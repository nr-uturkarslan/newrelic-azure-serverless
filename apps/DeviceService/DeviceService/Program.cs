using DeviceService.Commons.Constants;

var builder = WebApplication.CreateBuilder(args);

// Get environment variables.
GetEnvironmentVariables();

// Add services to the container.
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

app.Run();

void GetEnvironmentVariables()
{
    Console.WriteLine("Getting environment variables...");

    var cosmosDbUri = Environment.GetEnvironmentVariable("COSMOS_DB_URI");
    if (string.IsNullOrEmpty(cosmosDbUri))
    {
        Console.WriteLine("[COSMOS_DB_URI] is not provided");
        Environment.Exit(1);
    }
    EnvironmentVariables.COSMOS_DB_URI = cosmosDbUri;

    var cosmosDbName = Environment.GetEnvironmentVariable("COSMOS_DB_NAME");
    if (string.IsNullOrEmpty(cosmosDbUri))
    {
        Console.WriteLine("[COSMOS_DB_NAME] is not provided");
        Environment.Exit(1);
    }
    EnvironmentVariables.COSMOS_DB_NAME = cosmosDbName;

    var cosmosDbContainerName = Environment.GetEnvironmentVariable("COSMOS_DB_CONTAINER_NAME");
    if (string.IsNullOrEmpty(cosmosDbUri))
    {
        Console.WriteLine("[COSMOS_DB_CONTAINER_NAME] is not provided");
        Environment.Exit(1);
    }
    EnvironmentVariables.COSMOS_DB_CONTAINER_NAME = cosmosDbContainerName;

}
