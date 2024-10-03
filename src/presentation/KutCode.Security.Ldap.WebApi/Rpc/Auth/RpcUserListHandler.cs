using FastEndpoints;
using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Models;
using KutCode.Security.Ldap.Rpc;
using KutCode.Security.Ldap.WebApi.Services.Ldap;
using Serilog;

namespace KutCode.Security.Ldap.WebApi.Rpc.Auth;

public sealed class RpcUserListHandler : ICommandHandler<LdapGetUserListCommand, List<LdapUserData>>
{
	private readonly LdapGroupSearchService _ldapService;
	public RpcUserListHandler(LdapGroupSearchService ldapService)
	{
		_ldapService = ldapService;
	}
	public Task<List<LdapUserData>> ExecuteAsync(LdapGetUserListCommand command, CancellationToken ct)
	{
		Log.Information("RPC: Sending LDAP users load request");
		return Task.FromResult(_ldapService.GetUsers());
	}
}