using Carter;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);    
}).UseLightweightSessions();

var app = builder.Build();

// HTTP Pipelines

app.MapCarter();
app.Run();
