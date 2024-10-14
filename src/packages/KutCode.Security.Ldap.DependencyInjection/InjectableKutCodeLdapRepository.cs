
using KutCode.Security.Ldap.DependencyInjection.Static;

namespace KutCode.Security.Ldap.DependencyInjection;

public sealed class InjectableKutCodeLdapRepository : KutCodeLdapRepository
{
	public InjectableKutCodeLdapRepository(IHttpClientFactory httpClientFactory)
	{
		this.Client = httpClientFactory.CreateClient(HttpClientNames.HttpClientName);
	}
	
	public InjectableKutCodeLdapRepository(IHttpClientFactory httpClientFactory, string clientName)
	{
		this.Client = httpClientFactory.CreateClient(clientName);
	}
}