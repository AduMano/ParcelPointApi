global using ParcelPointApi.Models;
using ParcelPointApi.Hubs;
using ParcelPointApi.Services;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;

// Load environment variables from .env file
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Model Services Registration
builder.Services.AddDbContext<ParcelPointDbContext>();

// Shared Connection

builder.Services.AddSingleton<UserConnectionManager>(); // Register globally

// Register SignalR
builder.Services.AddSignalR();

// In Program.cs (ASP.NET Core)
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Avoid infinite loop references
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Automatically register all services
builder.Services.RegisterServices();
builder.Services.AddScoped<PasswordHelper>();

// Cors
builder.Services.AddCors(o => o.AddPolicy("LowCorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddSignalR()
    .AddJsonProtocol(opts => {
        opts.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Register Normal Web Sockets
// Dictionaries to keep track of connected clients.
var espConnections = new ConcurrentDictionary<string, WebSocket>();
var adminConnections = new ConcurrentDictionary<string, WebSocket>();

// ✅ Configure Kestrel to Handle API (`http://localhost:5192`) and WebSockets (`https://localhost:7192`)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5192); 
});

var app = builder.Build();
Console.WriteLine(new PasswordHelper().HashPassword("asdasdasd"));

// Allow Cors
app.UseCors("LowCorsPolicy");
app.UseRouting();

// Configure SignalR Paths
app.UseEndpoints(endpoints =>
{
    // Map the hub endpoint
    endpoints.MapHub<BaseHub>("/baseHub");  // <-- Define the URL here
    endpoints.MapHub<HomeHub>("/homeHub");  // <-- Define the URL here
    endpoints.MapHub<LockerHub>("/lockerHub");  // <-- Define the URL here
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.Run();
