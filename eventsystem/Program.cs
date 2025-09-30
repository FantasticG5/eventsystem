using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// DbContext via appsettings: ConnectionStrings:DefaultConnection
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// ===== DataProtection (delad nyckelring mellan tjänster) =====
var keysPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "CoreGym", "dp-keys"
);
Directory.CreateDirectory(keysPath);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("CoreGym"); // MÅSTE matcha AuthSystem

builder.Services.AddScoped<ITrainingClassService, TrainingClassService>();
builder.Services.AddScoped<ITrainingClassRepository, TrainingClassRepository>();


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

// ===== CORS: hårdkodat origin =====
const string SpaCors = "spa";
string[] allowedOrigins = { "https://fantasticg5-dmdbeshvcmfxe6ey.northeurope-01.azurewebsites.net", "http://localhost:5173", "https://localhost:5173" };

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(SpaCors, p => p
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

var app = builder.Build();


app.UseAuthentication();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = string.Empty;
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Service Api");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // /openapi/v1.json + auto UI om du k�r via VS/HTTP REPL etc.
}

app.UseHttpsRedirection();
app.UseCors(SpaCors); 
app.UseAuthorization();
app.MapControllers();

// Apply EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    await db.Database.MigrateAsync();
}

app.Run();
