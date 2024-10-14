using FastEndpoints;
using KutCode.Security.Ldap.DependencyInjection.Models;
using KutCode.Security.Ldap.DependencyInjection.Static;
using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Interfaces;
using KutCode.Security.Ldap.Models;
using KutCode.Security.Ldap.Rpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KutCode.Security.Ldap.DependencyInjection;

public static class DependencyInjection
{
	/// <summary>
	/// Inject KutCode LDAP repository with interface <see cref="IKutCodeLdapRepository"/>
	/// </summary>
	/// <param name="configurationSection">Configuration of type <see cref="LdapRepositoryInjectionConfiguration"/></param>
	/// <returns>IServiceCollection</returns>
	public static IServiceCollection AddKutCodeLdapRepository(
		this IServiceCollection services,
		IConfigurationSection configurationSection,
		ServiceLifetime lifetimeType)
	{
		var configuration = configurationSection.Get<LdapRepositoryInjectionConfiguration>()!;
		services.Configure<LdapRepositoryInjectionConfiguration>(configurationSection);
		return AddKutCodeLdapRepository(services, configuration, lifetimeType);
	}
	/// <summary>
	/// Inject KutCode LDAP repository with interface <see cref="IKutCodeLdapRepository"/>
	/// </summary>
	/// <returns>IServiceCollection</returns>
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

	#region Keyed

	/// <summary>
	/// Inject Keyed KutCode LDAP repository with interface <see cref="IKutCodeLdapRepository"/>
	/// </summary>
	/// <param name="configurationSection">Configuration of type <see cref="LdapRepositoryInjectionConfiguration"/></param>
	/// <returns>IServiceCollection</returns>
	public static IServiceCollection AddKeyedKutCodeLdapRepository(
		this IServiceCollection services,
		string key,
		IConfigurationSection configurationSection,
		ServiceLifetime lifetimeType)
	{
		var configuration = configurationSection.Get<LdapRepositoryInjectionConfiguration>()!;
		return AddKeyedKutCodeLdapRepository(services, key, configuration, lifetimeType);
	}
	/// <summary>
	/// Inject Keyed KutCode LDAP repository with interface <see cref="IKutCodeLdapRepository"/>
	/// </summary>
	/// <returns>IServiceCollection</returns>
	public static IServiceCollection AddKeyedKutCodeLdapRepository(
		this IServiceCollection services, 
		string key,
		LdapRepositoryInjectionConfiguration configuration, 
		ServiceLifetime lifetimeType)
	{
		services.AddHttpClient(key, client => {
			client.BaseAddress = new Uri(configuration.BaseUrl.TrimEnd(' ', '/'));
		});
		Func<IServiceProvider, object?, object> factory = (provider, _)
			=> new InjectableKutCodeLdapRepository(provider.GetRequiredService<IHttpClientFactory>(), key);
		services.Add(new ServiceDescriptor(typeof(IKutCodeLdapRepository), key, factory, lifetimeType));
		return services;
	}

	#endregion
	
	/// <summary>
	/// Use this method to configure RPC calls to LDAP service. <br/> 
	/// Settings, configured in <see cref="AddKutCodeLdapRepository"/> method, will be used there (like <see cref="RpcBaseUrl">RpcBaseUrl</see> option).
	/// </summary>
	public static WebApplication MapKutCodeLdapRpc(this WebApplication app)
	{
		var configuration = app.Services.GetRequiredService<IOptions<LdapRepositoryInjectionConfiguration>>().Value;
		return MapKutCodeLdapRpc(app, configuration.RpcBaseUrl);
	}
	
	/// <summary>
	/// Use this method to configure RPC calls to LDAP service. <br/> 
	/// Rewrites preconfigured in <see cref="AddKutCodeLdapRepository"/> method RpcBaseUrl setting string.
	/// </summary>
	public static WebApplication MapKutCodeLdapRpc(this WebApplication app, string? rpcBaseUrl)
	{
		if (string.IsNullOrEmpty(rpcBaseUrl)) 
			throw new ArgumentException($"Configuration url for {nameof(rpcBaseUrl)} is null or empty");
		app.MapRemote(rpcBaseUrl, c => {
			c.Register<LdapAuthCommand, LdapAuthenticationResponse>();
			c.Register<LdapGetUserListCommand, List<LdapUserData>>();
		});
		return app;
	}
}