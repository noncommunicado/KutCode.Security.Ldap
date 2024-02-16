
using KutCode.Security.Ldap.Models;

namespace KutCode.Security.Ldap.WebApi.Configuration;

public static class LdapConfiguration
{
	public static WebApplicationBuilder ConfigureLdap(this WebApplicationBuilder builder)
	{
		builder.Services.Configure<LdapServiceConfig>(builder.Configuration.GetSection("Ldap"));
		builder.Services.AddSingleton<LdapService>();
		return builder;
	}
}