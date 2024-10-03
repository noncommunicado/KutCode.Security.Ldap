
using KutCode.Security.Ldap.Models;
using KutCode.Security.Ldap.WebApi.Services.Ldap;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Ldap.Tests.Mocks;

public sealed class LdapServiceMock
{
	public static LdapService GetLdapServiceMock() {
		var settings = JsonConvert.DeserializeObject<LdapServiceConfig>(
			File.ReadAllText("appsettings.Development.json"));
		IOptions<LdapServiceConfig> ioptions = Options.Create<LdapServiceConfig>(settings);
		var connMngr = new LdapConnectionManager(ioptions);
		return new LdapService(ioptions, connMngr);
	}
	
	public static LdapGroupSearchService GetLdapGroupSearchServiceMock() {
		var settings = JsonConvert.DeserializeObject<LdapServiceConfig>(
			File.ReadAllText("appsettings.Development.json"));
		IOptions<LdapServiceConfig> ioptions = Options.Create<LdapServiceConfig>(settings);
		return new LdapGroupSearchService(ioptions);
	}
}