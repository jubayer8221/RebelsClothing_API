
using Microsoft.EntityFrameworkCore;
using RebelsClothing.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CORS configuration (allow your frontend apps)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",   // Vite
                "http://localhost:3000",   // Next.js
                "https://rebels-admin-dashboard.vercel.app"
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ✅ Bind server to port 1920 for all networks
//builder.WebHost.UseUrls("http://0.0.0.0:1920");
//var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
//builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// ✅ Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// ✅ Swagger (only in development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ❌ IMPORTANT: REMOVE HTTPS REDIRECTION (causes ERR_EMPTY_RESPONSE)
// app.UseHttpsRedirection();

// ✅ Enable CORS BEFORE controllers
app.UseCors("AllowFrontend");

app.UseAuthorization();

// ✅ Map controllers
app.MapControllers();

app.Run();