using FastEndpoints;
using KutCode.Security.Ldap.DependencyInjection.Models;
using KutCode.Security.Ldap.DependencyInjection.Static;
using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Interfaces;
using KutCode.Security.Ldap.Rpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KutCode.Security.Ldap.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddKutCodeLdapRepository(
		this IServiceCollection services, 
		IConfigurationSection configurationSection, 
		ServiceLifetime lifetimeType)
	{
		var configuration = configurationSection.Get<LdapRepositoryInjectionConfiguration>()!;
		return AddKutCodeLdapRepository(services, configuration, lifetimeType);
	}
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
	
	public static WebApplication MapKutCodeLdapRpc(this WebApplication app, IConfigurationSection configurationSection)
	{
		var configuration = configurationSection.Get<LdapRepositoryInjectionConfiguration>()!;
		return MapKutCodeLdapRpc(app, configuration);
	}
	public static WebApplication MapKutCodeLdapRpc(this WebApplication app, LdapRepositoryInjectionConfiguration configuration)
	{
		if (string.IsNullOrEmpty(configuration.RpcBaseUrl)) throw new ArgumentException($"Configuration url for {nameof(configuration.RpcBaseUrl)} is null or empty");
		app.MapRemote(configuration.RpcBaseUrl, c => {
			c.Register<LdapAuthCommand, LdapAuthenticationResponse>();
		});
		return app;
	}
}