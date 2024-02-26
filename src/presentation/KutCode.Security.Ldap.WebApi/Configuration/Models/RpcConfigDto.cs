
namespace KutCode.Security.Ldap.WebApi.Configuration.Models;

public sealed class RpcConfigDto
{
	public bool Enabled { get; set; }
	public int Port { get; set; }
	public bool Secure { get; set; }
}