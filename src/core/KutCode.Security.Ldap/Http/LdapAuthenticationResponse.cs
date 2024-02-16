using KutCode.Optionality;
using KutCode.Security.Ldap.Models;

namespace KutCode.Security.Ldap.Http;

public sealed class LdapAuthenticationResponse
{
	public LdapAuthenticationResponse() { }
	public LdapAuthenticationResponse(bool authorized, LdapUserData userData)
	{
		Authorized = authorized;
		UserData = userData;
	}
	public bool Authorized { get; set; }
	public LdapUserData? UserData { get; init; }
}