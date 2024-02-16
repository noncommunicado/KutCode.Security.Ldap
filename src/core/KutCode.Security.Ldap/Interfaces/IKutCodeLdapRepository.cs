using KutCode.Security.Ldap.Http;

namespace KutCode.Security.Ldap.Interfaces;

public interface IKutCodeLdapRepository
{
	Task<LdapAuthenticationResponse> LoginAsync(LdapLoginRequest request, CancellationToken ct = default);
}