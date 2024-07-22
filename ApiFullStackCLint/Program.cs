using ApiFullStackCLint.Data.GraphQL.Query;
using ApiFullStackCLint.Data.MongoDB;

var builder = WebApplication.CreateBuilder(args);

// Load MongoDB settings from appsettings.json
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

// Register MongoDB settings with DI container
builder.Services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

// Register MongoDB service with DI container
builder.Services.AddSingleton<MongoDbService>();

// Add GraphQL server, CORS, and other services as needed
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Configure Kestrel for HTTPS
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000); // HTTP port
    serverOptions.ListenAnyIP(5001, listenOptions =>
    {
        // Path to the PFX certificate and its password
        const string certificatePath = "/root/persistentStorage/cert/fullstackclint.com.pfx";
        const string certificatePassword = "password";
        listenOptions.UseHttps(certificatePath, certificatePassword);
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();