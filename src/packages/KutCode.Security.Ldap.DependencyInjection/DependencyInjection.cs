using KutCode.Security.Ldap.DependencyInjection.Models;
using KutCode.Security.Ldap.DependencyInjection.Static;
using KutCode.Security.Ldap.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Security.Ldap.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddKutCodeLdapRepository(
		this IServiceCollection services, 
		LdapRepositoryInjectionConfiguration configuration, 
		ServiceLifetime lifetimeType)
	{
		services.AddHttpClient(HttpClientNames.HttpClientName, client => {
			client.BaseAddress = new Uri(configuration.BaseUrl.TrimEnd(' ', '/'));
		});
		
		Func<IServiceProvider, object> factory = (provider) =>
			new InjectableKutCodeLdapRepository(provider.GetRequiredService<IHttpClientFactory>());
		services.Add(new ServiceDescriptor(typeof(IKutCodeLdapRepository), factory, lifetimeType));

		return services;
	}

	public static IServiceCollection AddKutCodeLdapRepository(
		this IServiceCollection services, 
		IConfigurationSection configurationSection, 
		ServiceLifetime lifetimeType)
	{
		var configuration = configurationSection.Get<LdapRepositoryInjectionConfiguration>()!;
		services.AddHttpClient(HttpClientNames.HttpClientName, client => {
			client.BaseAddress = new Uri(configuration.BaseUrl.TrimEnd(' ', '/'));
		});
		
		Func<IServiceProvider, object> factory = (provider) =>
			new InjectableKutCodeLdapRepository(provider.GetRequiredService<IHttpClientFactory>());
		services.Add(new ServiceDescriptor(typeof(IKutCodeLdapRepository), factory, lifetimeType));

		return services;
	}
}