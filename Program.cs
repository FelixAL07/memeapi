using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyCorsPolicy",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .WithMethods("GET");
        });
});

// Configure Kestrel to use HTTP in production
if (builder.Environment.IsProduction())
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        // Listen on port 8080 (or your preferred port) for HTTP
        serverOptions.ListenAnyIP(8081);
    });
}

var app = builder.Build();

app.UseCors("MyCorsPolicy");
app.MapControllers();
app.MapOpenApi();
app.MapScalarApiReference();

app.Run();