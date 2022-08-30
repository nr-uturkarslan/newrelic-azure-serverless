using ArchiveService.Commons.Constants;

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

    var blobContainerUri = Environment.GetEnvironmentVariable("BLOB_CONTAINER_URI");
    if (string.IsNullOrEmpty(blobContainerUri))
    {
        Console.WriteLine("[BLOB_CONTAINER_URI] is not provided");
        Environment.Exit(1);
    }
    EnvironmentVariables.BLOB_CONTAINER_URI = blobContainerUri;
}

