using FastEndpoints;
using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Models;

namespace KutCode.Security.Ldap.Rpc;

public class LdapGetUserListCommand : ICommand<List<LdapUserData>>
{
}