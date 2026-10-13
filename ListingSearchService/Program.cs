using ListingSearchEngine.Search;

var builder = WebApplication.CreateBuilder(args);

// Configure logging providers
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddControllers();
// Register engine services
builder.Services.AddSingleton<ListingSearchData.Repositories.IListingRepository, ListingSearchData.Repositories.ListingRepository>();
builder.Services.AddSingleton<ListingSearchEngine.Search.Interfaces.IFilterService, ListingSearchEngine.Search.FilterService>();
builder.Services.AddSingleton<ListingSearchEngine.Search.Interfaces.IScorer, ListingSearchEngine.Search.Scorer>();
builder.Services.AddSingleton<SearchService>();
// Configure scoring options from configuration
builder.Services.Configure<ListingSearchEngine.Config.ScoringOptions>(builder.Configuration.GetSection("Scoring"));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Global exception handler: log and return a safe JSON response
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var exception = feature?.Error;
        var logger = context.RequestServices.GetService<Microsoft.Extensions.Logging.ILoggerFactory>()?.CreateLogger("GlobalExceptionHandler");
        if (exception != null)
        {
            logger?.LogError(exception, "Unhandled exception occurred while processing request {Path}", context.Request.Path);
        }

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ListingSearch API V1");
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// Serve SPA index
app.MapFallbackToFile("index.html");

app.Run();
