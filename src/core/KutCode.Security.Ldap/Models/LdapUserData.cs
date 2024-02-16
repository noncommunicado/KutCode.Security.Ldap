namespace KutCode.Security.Ldap.Models;

public sealed class LdapUserData
{
	public string UserId { get; set; }
	public string UserDistinguishedName { get; set; }
	public string UserDisplayName { get; set; }
	public string UserEmail { get; set; }
	public List<string> MemberOfGroups { get; set; } = new();
}