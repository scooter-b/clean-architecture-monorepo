using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// register the OpenAPI service for the document to be generated
builder.Services.AddOpenApi();

// Add YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Expose the actual JSON document (defaults to /openapi/v1.json)
    app.MapOpenApi();

    // Map Scalar UI
    app.MapScalarApiReference("/scalar", options =>
    {
        var gateway = "http://localhost:5000";
        var docs = "docs";
        var openApi = "openapi/v1.json";

        // Points Scalar to the document proxied through your YARP route
        options.WithOpenApiRoutePattern($"{gateway}/{docs}/user/{openApi}");

        options.WithTitle("Gateway API Reference")
               .WithTheme(ScalarTheme.DeepSpace);
    });

    // Redirect root to Scalar (use no trailing slash to avoid load errors)
    app.MapGet("/", () => Results.Redirect("/scalar/v1", permanent: false));
}

// Enable the proxy middleware
app.MapReverseProxy();

app.Run();

