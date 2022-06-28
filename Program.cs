using AppAndroid;
using AppAndroid.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
await app.MigrateDatabase<AppContext>();

app.Run();