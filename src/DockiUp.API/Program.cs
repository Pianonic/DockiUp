using DockiUp.Application.Models;
using DockiUp.Application.Queries;
using DockiUp.Application.Services;
using DockiUp.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddSpaStaticFiles(spaStaticFilesOptions => { spaStaticFilesOptions.RootPath = "wwwroot/browser"; });

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AppQuery).Assembly));

// Add the WebhookSecretService with scoped lifetime
builder.Services.AddScoped<IWebhookSecretService, WebhookSecretService>();

// Configure the DbContext with a connection string.
builder.Services.AddDbContext<DockiUpDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DockiUpDatabase"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DockiUpDatabase")),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)
    ));

// Configure SystemPaths
builder.Services.Configure<SystemPaths>(builder.Configuration.GetSection("SystemPaths"));

// Register Swagger services
builder.Services.AddSwaggerGen();  // This is the correct method for adding Swagger

// Add CORS policy to allow all origins, methods, and headers
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(corsBuilder =>
        {
            corsBuilder.WithOrigins("http://localhost:4200");
            corsBuilder.WithExposedHeaders("Content-Disposition");
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowCredentials();
            if (!builder.Environment.IsProduction())
            {
                corsBuilder.WithExposedHeaders("X-Impersonate");
            }
        });
    });
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DockiUpDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DockiUp API V1");  // You can modify this URL as per your versioning
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

// Ensure frontend routes work
app.UseRouting();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

// Serve Angular Frontend in Production
if (!app.Environment.IsDevelopment())
{
    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "wwwroot";
    });
}

app.Run();