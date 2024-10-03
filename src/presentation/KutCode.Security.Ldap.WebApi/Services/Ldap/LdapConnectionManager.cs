using KutCode.Security.Ldap.Models;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace KutCode.Security.Ldap.WebApi.Services.Ldap;

/// <summary>
/// Should be used as Singleton in DI
/// </summary>
public sealed class LdapConnectionManager : IDisposable
{
	private readonly IOptions<LdapServiceConfig> _ldapOptions;
	public LdapConnectionManager(IOptions<LdapServiceConfig> ldapOptions)
	{
		_ldapOptions = ldapOptions;
	}

	private object _locker = new();
	private LdapConnection? _connection;

	public LdapConnection GetConnection()
	{
		lock (_locker) return Get();
	}
	private LdapConnection Get()
	{
		if (_connection is not null)
			return (_connection.Clone() as LdapConnection)!;
		_connection = new LdapConnection();
		_connection.Connect(_ldapOptions.Value.Server, _ldapOptions.Value.ServerPort);
		_connection.SecureSocketLayer = _ldapOptions.Value.UseSsl;
		return _connection;
	}

	~LdapConnectionManager()
	{
		_connection?.Dispose();
	}
	public void Dispose()
	{
		_connection?.Dispose();
		GC.SuppressFinalize(this);
	}
}