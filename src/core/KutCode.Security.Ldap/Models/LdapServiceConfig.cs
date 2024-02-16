
namespace KutCode.Security.Ldap.Models;

public sealed class LdapServiceConfig
{
	public string Server { get; set; }
	public int ServerPort { get; set; }
	public string DomainName { get; set; }
	public string BaseLdapFilter { get; set; }
	public string AdditionalLdapFilter { get; set; }
	public string LoginAttribute { get; set; }
	public string DisplayNameAttribute { get; set; }
	public string EmailAttribute { get; set; }
	public bool UseSsl { get; set; }
}