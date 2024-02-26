
namespace KutCode.Security.Ldap.Http;

public class LdapLoginRequest
{
	public LdapLoginRequest() { }

	public LdapLoginRequest(string login, string password)
	{
		Login = login;
		Password = password;
	}
	
	public string Login { get; set; }
	public string Password { get; set; }
}