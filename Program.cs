using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Uploader_Api.Context;
using Uploader_Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Uploader_Api.Services.Concrete;
using Uploader_Api.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Uploader_Api.Middleware;
using Uploader_Api.Helpers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//Microsoft Identity config
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IPostService, PostService>();

//builder.Services.AddScoped<AuthMiddleware>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt : Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt: Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
           .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
           .RequireAuthenticatedUser()
           .Build();
});


//builder.Services.AddScoped<JwtSecurityHandlerWrapper>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme 
    { 
        In = ParameterLocation.Header,
        Name= "Authorization",
        Type = SecuritySchemeType.ApiKey
    
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddScoped<CustomTokenHandler>();


var app = builder.Build();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), builder =>
//{
//    builder.UseMiddleware<AuthMiddleware>(); // Apply middleware to /api/* endpoints
//});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new [] { "Admin", "User" };
    foreach(var role in roles)
    {
        if (!await rolemanager.RoleExistsAsync(role))
        {
            await rolemanager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();
