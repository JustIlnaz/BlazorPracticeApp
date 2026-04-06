using BlazorPracticeApp.Api.ContextDatabase;
using BlazorPracticeApp.Api.Hubs;
using BlazorPracticeApp.Api.Interfaces;
using BlazorPracticeApp.Api.JWT;
using BlazorPracticeApp.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ContextDb>(p => p.UseNpgsql(builder.Configuration.GetConnectionString("StringDB")), ServiceLifetime.Scoped);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMovieService, MovieService>();

builder.Services.AddSingleton<JwtGenerator>();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazorWasm");

app.UseAuthorization();

app.MapHub<ChatHub>("/chathub");
app.MapControllers();

app.Run();
