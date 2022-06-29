using System.Text;
using AppAndroid;
using AppAndroid.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AppContext = AppAndroid.AppContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AdicionarAutenticacaoSwagger();

var connection = DatabaseConnection.ConfigureDatabaseConnection(builder.Environment.EnvironmentName);

builder.Services.AddDbContext<AppContext>(opt =>
{
    if (connection == null)
        connection = builder.Configuration.GetConnectionString("DATABASE_URL");
    opt.UseNpgsql(connection);
})
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppContext>();

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;

        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TOKEN_CONFIGURATION_KEY") ??
                                          string.Empty);
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
    
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
await app.MigrateDatabase<AppContext>();

app.Run();