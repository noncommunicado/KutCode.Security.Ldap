
using KutCode.Optionality;

namespace KutCode.Security.Ldap.WebApi.Ldap;

public sealed record LdapAuthentication(bool Authorized)
{
	public LdapAuthentication(bool authorized, LdapUserData userData) : this(authorized)
	{
		UserData = userData;
	}
	public Optional<LdapUserData> UserData { get; init; } = Optional.None<LdapUserData>();
}

public sealed class LdapUserData
{
	public string UserId { get; set; }
	public string UserDistinguishedName { get; set; }
	public string UserDisplayName { get; set; }
	public List<string> MemberOfGroups { get; set; } = new();
}