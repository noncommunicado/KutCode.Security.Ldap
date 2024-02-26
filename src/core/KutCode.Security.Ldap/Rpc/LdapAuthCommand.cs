using FastEndpoints;
using KutCode.Security.Ldap.Http;

namespace KutCode.Security.Ldap.Rpc;

public class LdapAuthCommand : LdapLoginRequest, ICommand<LdapAuthenticationResponse>
{
}