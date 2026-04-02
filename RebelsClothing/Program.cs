//using Microsoft.EntityFrameworkCore;
//using RebelsClothing.Data;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


//// ✅ Add CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend",
//        policy =>
//        {
//            //policy
//            //    .WithOrigins("http://localhost:5173") // your Next.js
//            //    .WithOrigins("http://localhost:3000") // your Next.js app
//            //    .WithOrigins("https://rebels-admin-dashboard.vercel.app")
//            //    .AllowAnyMethod()
//            //    .AllowAnyHeader();
//            policy
//            .WithOrigins(
//            "http://localhost:5173",
//            "https://rebels-admin-dashboard.vercel.app"
//)
//.AllowAnyMethod()
//.AllowAnyHeader();
//        });
//});

//builder.WebHost.UseUrls("http://0.0.0.0:1920");

//// ✅ Add DB context
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
//);

//var app = builder.Build();

//// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//// ✅ Use CORS here BEFORE MapControllers
//app.UseCors("AllowFrontend");

//app.UseAuthorization();

//app.MapControllers();

//app.Run();


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
builder.WebHost.UseUrls("http://0.0.0.0:1920");

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