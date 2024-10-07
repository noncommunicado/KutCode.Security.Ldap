using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Models;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using Serilog;
using LdapConnection = Novell.Directory.Ldap.LdapConnection;

namespace KutCode.Security.Ldap.WebApi.Services.Ldap;

public sealed class LdapService
{
	private readonly IOptions<LdapServiceConfig> _ldapOptions;
	private readonly LdapConnectionManager _cm;
	public LdapService(IOptions<LdapServiceConfig> ldapOptions, LdapConnectionManager cm)
	{
		_ldapOptions = ldapOptions;
		_cm = cm;
	}

	public LdapAuthenticationResponse Authenticate(string login, string password)
	{
		var userName = LdapUtils.GetUsernameForAuthentication(login, _ldapOptions.Value.DomainName);
		using var conn = _cm.GetConnection();
		try {
			conn.Bind(userName, password);
		}
		catch (Exception ex) { // if failed to auth -> credentials incorrect
			Log.Error(ex, "Failed to authorize in {LdapServer}:{Port}", _ldapOptions.Value.Server, _ldapOptions.Value.ServerPort);
			return new LdapAuthenticationResponse {Authorized = false };
		}
		var filter = LdapUtils.GetLdapFilter(_ldapOptions.Value.AdditionalLdapFilter, _ldapOptions.Value.LoginAttribute, login);
		var queue = conn.Search(_ldapOptions.Value.BaseLdapFilter, LdapConnection.ScopeSub, filter, new string[] {}, false, null, null);
		
		var me = conn.WhoAmI();
		LdapAuthenticationResponse result = new(true, new LdapUserData {
			UserId = me.Id
		});
		LdapMessage responseMessage;
		var displayNameAttributeNormalized = _ldapOptions.Value.DisplayNameAttribute.Trim().ToLower();
		var emailAttributeNormalized = _ldapOptions.Value.EmailAttribute.Trim().ToLower();
		while ((responseMessage = queue.GetResponse()) != null) {
			if (responseMessage is not LdapSearchResult sRes) continue;

			var entry = sRes.Entry;
			var attrs = entry.GetAttributeSet();
			result.UserData!.UserDistinguishedName ??= attrs.FirstOrDefault(x => x.Key.ToLower() == "distinguishedname").Value.StringValue;

			if (attrs.Any(x => x.Key.ToLower() == "memberof")) {
				var groups = attrs.FirstOrDefault(x => x.Key.ToLower() == "memberof").Value;
				foreach (var groupLdapPath in groups.StringValueArray ?? ArraySegment<string>.Empty)
					result.UserData.MemberOfGroups.Add(LdapUtils.GetFriendlyName(groupLdapPath));
			}

			// get display name
			if (attrs.Any(x => x.Key.ToLower() == displayNameAttributeNormalized))
			{
				var displayNameAttrValue = attrs.FirstOrDefault(x => x.Key.ToLower() == displayNameAttributeNormalized).Value;
				if (displayNameAttrValue is not null)
					result.UserData!.UserDisplayName ??= displayNameAttrValue.StringValue;
			}
			// get email
			if (attrs.Any(x => x.Key.ToLower() == emailAttributeNormalized))
			{
				var attrValue = attrs.FirstOrDefault(x => x.Key.ToLower() == emailAttributeNormalized).Value;
				if (attrValue is not null)
					result.UserData!.UserEmail ??= attrValue.StringValue;
			}
		}

		return result;
	}
}