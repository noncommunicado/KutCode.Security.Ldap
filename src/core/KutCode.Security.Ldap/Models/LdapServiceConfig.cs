
using System.ComponentModel;

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
	
	[Description("Fallback attribute for email")]
	public string EmailAttributeV2 { get; set; }
	
	[Description("One more fallback attribute for email")]
	public string EmailAttributeV3 { get; set; }
	
	public bool UseSsl { get; set; }

	public LdapServiceAccountCredentials ServiceAccount { get; set; }
	
	public class LdapServiceAccountCredentials
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}