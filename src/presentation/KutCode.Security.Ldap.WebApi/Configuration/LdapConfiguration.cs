
using KutCode.Security.Ldap.Models;
using KutCode.Security.Ldap.WebApi.Services.Ldap;

namespace KutCode.Security.Ldap.WebApi.Configuration;

public static class LdapConfiguration
{
	public static WebApplicationBuilder ConfigureLdap(this WebApplicationBuilder builder)
	{
		builder.Services.Configure<LdapServiceConfig>(builder.Configuration.GetSection("Ldap"));
		builder.Services.AddSingleton<LdapConnectionManager>();
		builder.Services.AddSingleton<LdapService>();
		builder.Services.AddSingleton<LdapGroupSearchService>();
		return builder;
	}
}