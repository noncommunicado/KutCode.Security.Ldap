using KutCode.Security.Ldap.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.GetRequiredSection("Ldap");
builder.Services
	.AddKutCodeLdapRepository(config, ServiceLifetime.Scoped);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.Run();