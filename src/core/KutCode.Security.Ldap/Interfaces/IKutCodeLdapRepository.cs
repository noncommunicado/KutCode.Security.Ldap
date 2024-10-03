using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Models;

namespace KutCode.Security.Ldap.Interfaces;

public interface IKutCodeLdapRepository
{
	Task<LdapAuthenticationResponse> LoginAsync(LdapLoginRequest request, CancellationToken ct = default);
	Task<HttpResponseBase<List<LdapUserData>>> GetUsersAsync(CancellationToken ct = default);
}