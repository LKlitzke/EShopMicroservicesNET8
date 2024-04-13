using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Reverse Proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Rate Limiter
builder.Services.AddRateLimiter(rateLimOptions =>
{
    rateLimOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

var app = builder.Build();

app.UseRateLimiter();
app.MapReverseProxy();
app.Run();

/* YARP
    Route > Cluster > Path > Destination
    catalog-route > catalog-cluster > /catalog-service/{**catch-all} > http://localhost:6000/ (docker)

    https://localhost:5054/catalog-service/products ---> http://localhost:6000/products
    https://localhost:5054/basket-service/basket/swn ---> http://localhost:6000/basket/swn
*/