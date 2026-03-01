using BlazorPracticeApp.Api.ContextDatabase;
using BlazorPracticeApp.Api.Interfaces;
using BlazorPracticeApp.Api.JWT;
using BlazorPracticeApp.Api.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ContextDb>(p => p.UseNpgsql(builder.Configuration.GetConnectionString("StringDB")), ServiceLifetime.Scoped);
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<JwtGenerator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm", policy =>
    {
        policy.WithOrigins("http://localhost:5070").AllowAnyHeader().AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
