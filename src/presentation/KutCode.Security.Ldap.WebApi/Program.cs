using FastEndpoints;
using FastEndpoints.Swagger;
using KutCode.Security.Ldap.WebApi.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddFastEndpoints()
	.ConfigureCors()
	.ConfigureSwagger()
	.ConfigureSerilogging();

var app = builder.Build();

// app.UseSerilogRequestLogging();
app.UseStaticFiles();
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