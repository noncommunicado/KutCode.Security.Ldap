
namespace KutCode.Security.Ldap.WebApi.Endoints.Auth.Authorize;

public sealed class Request
{
	public string Login { get; set; }
	public string Password { get; set; }
}