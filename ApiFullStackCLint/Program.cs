using ApiFullStackClint.Data.GraphQL.Query;
using ApiFullStackClint.Data.MongoDb;

var builder = WebApplication.CreateBuilder(args);


// Load settings from appsettings.ENVIRONMENT.json
builder.Configuration
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

Console.WriteLine("Enviroment name = " + builder.Environment.EnvironmentName);

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
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Configure Kestrel server options based on environment
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    var certPath = builder.Configuration["Server:Endpoints:Https:Certificate:Path" ?? string.Empty];
    var certPassword = builder.Configuration["Server:Endpoints:Https:Certificate:KeyPassword" ?? string.Empty];
    var httpPort = int.TryParse(builder.Configuration["Server:Endpoints:Http:Url"], out var parsedHttpPort) ? parsedHttpPort : 5142;
    var httpsPort = int.TryParse(builder.Configuration["Server:Endpoints:Https:Url"], out var parsedHttpsPort) ? parsedHttpsPort : 5001;
    
     if (builder.Environment.IsDevelopment())
     {
         // Development settings (HTTP only)
         serverOptions.ListenAnyIP(httpPort);
     }
     else
     {
         // Production settings (HTTP and HTTPS)
         serverOptions.ListenAnyIP(httpPort);
         serverOptions.ListenAnyIP(httpsPort, listenOptions =>
         {
             if (!string.IsNullOrWhiteSpace(certPath) && !string.IsNullOrWhiteSpace(certPassword))
             {
                 listenOptions.UseHttps(certPath, certPassword);
             }
             else
             {
                 throw new InvalidOperationException("Certificate path or password is not configured.");
             }
         });
     }
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseRouting();
app.UseCors("AllowAll");

app.MapGraphQL(); // Register your GraphQL endpoint

app.Run();