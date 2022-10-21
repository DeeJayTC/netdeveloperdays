using Microsoft.IdentityModel.Logging;
using netDeveloperDays;
using System.Reflection;
using TCDev.APIGenerator;
using TCDev.APIGenerator.Extension;
using TCDev.APIGenerator.Identity;
using TCDev.APIGenerator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add API Generator and load data
builder.Services
    .AddApiGeneratorServices()
    .AddConfig(new ApiGeneratorConfig()
    {
        DatabaseOptions =new DatabaseOptions()
        {
            EnableAutomaticMigration = true,
            Connection = "Server=localhost;database=netDeveloperDays1;user=sa;password=youAreWeak123!",
            DatabaseType = DbType.Sql,
            ConnectionStringName = "ApiGeneratorDatabase"
        }
    })
    .AddAssemblyWithOData(Assembly.GetExecutingAssembly())
    .AddOData()
    .AddDataContextSQL()
    .AddRabbitMQ()
    .AddSwagger(true);

builder.Services.AddScoped<DeepLService>();

var app = builder.Build();

app.MapControllers();
app.UseAutomaticApiMigrations(true);
app.UseApiGenerator();

app.UseRouting();


app.UseEndpoints(endpoints =>
{
    endpoints.UseApiGeneratorEndpoints();
    endpoints.MapControllers();
});

app.Run();


//
//builder.Services.AddApiGeneratorIdentity(builder.Configuration);
//app.UseApiGeneratorAuthentication();