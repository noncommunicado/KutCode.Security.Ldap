
namespace KutCode.Security.Ldap.WebApi.Services.Ldap;

public sealed class LdapUtils
{
	public static string GetFriendlyName(string groupLdapPath)
	{
		var ldapPathPieces = groupLdapPath.Split(',');
		if (ldapPathPieces.Length <= 0) return string.Empty;
		var keyValue = ldapPathPieces[0].Split('=');
		if (keyValue.Length != 2) return string.Empty;
		return keyValue[1];
	}
	
	public static string GetUsernameForAuthentication(string username, string domainName)
	{
		if (username.Contains("@")) return username.Trim();
		return $"{username.Trim()}@{domainName}".Normalize();
	}
	public static string GetLdapFilter(string defaultFilter, string loginAttribute, string login)
	{
		if (string.IsNullOrWhiteSpace(loginAttribute))
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(loginAttribute));
		if (string.IsNullOrWhiteSpace(login))
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(login));
		if (string.IsNullOrWhiteSpace(defaultFilter))
			return $"({loginAttribute}={login})";
		return $"(&({defaultFilter})({loginAttribute}={login}))";
	}
}