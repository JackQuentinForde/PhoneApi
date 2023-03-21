using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false);

// Add services to the container.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    });
});

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "PhoneAPI", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "phone_api_test",
        ValidAudience = "https://localhost:7039",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ApiKey"]))
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();

builder.Services.AddScoped(s =>
    DbContextFactory.Create<PhoneDbContext>(
        builder.Configuration.GetConnectionString("PhoneDatabase"),
        !builder.Environment.IsDevelopment()
    ));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PhoneDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Phone API");
});

app.MapControllers();
app.Run();