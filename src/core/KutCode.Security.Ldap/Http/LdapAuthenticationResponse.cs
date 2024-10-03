using KutCode.Security.Ldap.Models;

namespace KutCode.Security.Ldap.Http;

public class LdapAuthenticationResponse
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