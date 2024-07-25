using ApiFullStackClint.Data.GraphQL.Query;
using ApiFullStackClint.Data.MongoDb;

var builder = WebApplication.CreateBuilder(args);

// Load MongoDB settings from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Register MongoDB settings and service with DI container
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbService>();

// Add GraphQL server, CORS, and other services
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

// Configure Kestrel server options based on environment
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Development settings (HTTP only)
        serverOptions.ListenAnyIP(5142); // HTTP port
    }
    else
    {
        // Production settings (HTTP and HTTPS)
        serverOptions.ListenAnyIP(5000); // HTTP port
        serverOptions.ListenAnyIP(5001, listenOptions =>
        {
            // Path to the PFX certificate and its password
            const string certificatePath = "/root/storage/cert/fullstackclint.com.pfx";
            const string certificatePassword = "password";
            listenOptions.UseHttps(certificatePath, certificatePassword);
        });
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapGraphQL(); // Register your GraphQL endpoint


app.Run();