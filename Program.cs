global using server.Model;
global using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Services.JokeService;
using server.Services.AuthService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<JokeContext>();
builder.Services.AddDbContext<AuthContext>();
builder.Services.AddDbContext<UserJokeLaughContext>();
builder.Services.AddScoped<IJokeService, JokeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<Authorization>();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowCredentials()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
