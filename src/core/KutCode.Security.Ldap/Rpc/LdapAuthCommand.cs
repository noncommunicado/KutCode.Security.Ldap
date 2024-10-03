using FastEndpoints;
using KutCode.Security.Ldap.Http;

namespace KutCode.Security.Ldap.Rpc;

public class LdapAuthCommand : LdapLoginRequest, ICommand<LdapAuthenticationResponse>
{
	public LdapAuthCommand() : base() { }
	public LdapAuthCommand(string login, string password) : base(login, password) { }
}