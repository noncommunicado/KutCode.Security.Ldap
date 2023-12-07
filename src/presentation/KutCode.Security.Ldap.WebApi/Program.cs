using FastEndpoints;
using FastEndpoints.Swagger;
using KutCode.Security.Ldap.WebApi.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine("appsettings","appsettings.json"), optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile(Path.Combine("appsettings",$"appsettings.{builder.Environment.EnvironmentName}.json"), optional: true, reloadOnChange: true);

builder.AddFastEndpoints()
	.ConfigureCors()
	.ConfigureSwagger()
	.ConfigureSerilogging()
	.ConfigureLdap();

var app = builder.Build();

app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();
app.UseCors(CorsConfiguration.CorsPolicyName);

app.UseDefaultExceptionHandler();
app.UseFastEndpoints(c => {
	c.Endpoints.RoutePrefix = "api";
	c.Versioning.Prefix = "v";
	c.Versioning.PrependToRoute = true;
	//c.Serializer.Options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
app.UseSwaggerGen();

Log.Information("! APPLICATION START !");
await app.RunAsync();