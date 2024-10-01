using FastEndpoints;
using FastEndpoints.Swagger;
using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Models;
using KutCode.Security.Ldap.Rpc;
using KutCode.Security.Ldap.WebApi.Configuration;
using KutCode.Security.Ldap.WebApi.Configuration.Models;
using KutCode.Security.Ldap.WebApi.Rpc.Auth;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine("appsettings", "appsettings.json"), optional: false,
	reloadOnChange: true);
builder.Configuration.AddJsonFile(
	Path.Combine("appsettings", $"appsettings.{builder.Environment.EnvironmentName}.json"), optional: true,
	reloadOnChange: true);

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
});


{ // rpc
	var rpcConfigService = app.Services.GetService<RpcConfigDto>();
	if (rpcConfigService is not null && rpcConfigService.Enabled) {
		app.MapHandlers(h => { h.Register<LdapAuthCommand, RpcAuthHandler, LdapAuthenticationResponse>(); });
		app.MapGrpcReflectionService();
	}
}

app.UseSwaggerGen();
app.UseHttpRequestsLogging();

await app.RunAsync();