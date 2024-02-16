using KutCode.Optionality;
using KutCode.Security.Ldap.Models;

namespace KutCode.Security.Ldap.Http;

public sealed record LdapAuthenticationResponse(bool Authorized)
{
	public LdapAuthenticationResponse(bool authorized, LdapUserData userData) : this(authorized)
	{
		UserData = userData;
	}
	public Optional<LdapUserData> UserData { get; init; } = Optional.None<LdapUserData>();
}