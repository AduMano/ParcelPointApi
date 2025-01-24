global using ParcelPointApi.Models;

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

var app = builder.Build();
Console.WriteLine(new PasswordHelper().HashPassword("asdasdasd"));

// Allow Cors
app.UseCors("LowCorsPolicy");
app.UseRouting();


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
