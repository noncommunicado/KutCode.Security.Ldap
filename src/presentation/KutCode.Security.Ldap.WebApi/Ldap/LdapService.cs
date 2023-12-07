using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace KutCode.Security.Ldap.WebApi.Ldap;

public sealed class LdapService
{
	private readonly IOptions<LdapServiceConfig> _ldapOptions;
	public LdapService(IOptions<LdapServiceConfig> ldapOptions)
	{
		_ldapOptions = ldapOptions;
	}

	public LdapAuthentication Authenticate(string login, string password)
	{
		using var ldap = GetConnection();
		try {
			ldap.Bind(GetUsernameForAuthentication(login), password);
		}
		catch { // if failed to auth -> credentials incorrect
			return new LdapAuthentication(false);
		}
		var filter = GetLdapFilter(_ldapOptions.Value.AdditionalLdapFilter, _ldapOptions.Value.LoginAttribute, login);
		var queue = ldap.Search(_ldapOptions.Value.BaseLdapFilter, LdapConnection.ScopeSub, filter, new string[] {}, false, null, null);

		var me = ldap.WhoAmI();
		LdapAuthentication result = new(true, new LdapUserData {
			UserId = me.Id
		});
		LdapMessage responseMessage;
		while ((responseMessage = queue.GetResponse()) != null) {
			if (responseMessage is LdapSearchResult sRes) {
				var entry = sRes.Entry;
				var attrs = entry.GetAttributeSet();
				result.UserData.Value!.UserDistinguishedName ??= attrs.FirstOrDefault(x => x.Key.ToLower() == "distinguishedname").Value.StringValue;
				
				var groups = attrs.FirstOrDefault(x => x.Key.ToLower() == "memberof").Value;
				foreach (var groupLdapPath in groups.StringValueArray)
					result.UserData.Value.MemberOfGroups.Add(GetFriendlyName(groupLdapPath));

				var displayNameAttributeNormalized = _ldapOptions.Value.DisplayNameAttribute.Trim().ToLower();
				if (attrs.Any(x => x.Key.ToLower() == displayNameAttributeNormalized))
				{
					var displayNameAttrValue = attrs.FirstOrDefault(x => x.Key.ToLower() == displayNameAttributeNormalized).Value;
					if (displayNameAttrValue is not null)
						result.UserData.Value!.UserDisplayName ??= displayNameAttrValue.StringValue;
				}
			}
		}

		return result;
	}

	private string GetFriendlyName(string groupLdapPath)
	{
		var ldapPathPieces = groupLdapPath.Split(',');
		if (ldapPathPieces.Length <= 0) return string.Empty;
		var keyValue = ldapPathPieces[0].Split('=');
		if (keyValue.Length != 2) return string.Empty;
		return keyValue[1];
	}

	private string GetUsernameForAuthentication(string username)
	{
		if (username.Contains("@")) return username.Trim();
		return $"{username.Trim().Normalize()}@{_ldapOptions.Value.DomainName}";
	}

	LdapConnection GetConnection()
	{
		var ldap = new LdapConnection();
		ldap.Connect(_ldapOptions.Value.Server, _ldapOptions.Value.ServerPort);
		ldap.SecureSocketLayer = _ldapOptions.Value.UseSsl;
		return ldap;
	}
	
	string GetLdapFilter(string defaultFilter, string loginAttribute, string login)
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