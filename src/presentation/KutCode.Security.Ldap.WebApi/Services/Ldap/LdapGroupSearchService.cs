using System.Collections;
using System.DirectoryServices.Protocols;
using System.Net;
using SearchOption = System.DirectoryServices.Protocols.SearchOption;
using KutCode.Security.Ldap.Models;
using Microsoft.Extensions.Options;

namespace KutCode.Security.Ldap.WebApi.Services.Ldap;

public sealed class LdapGroupSearchService
{
	private readonly IOptions<LdapServiceConfig> _ldapOptions;
	private readonly string _displayNameAttributeNormalized;
	private readonly string _loginAttributeNormalized;
	private readonly string _emailAttributeNormalized;
	private readonly string _emailAttributeNormalizedV2;
	private readonly string _emailAttributeNormalizedV3;
	public LdapGroupSearchService(IOptions<LdapServiceConfig> ldapOptions)
	{
		_ldapOptions = ldapOptions;
		_displayNameAttributeNormalized = _ldapOptions.Value.DisplayNameAttribute.Trim()?.ToLower() ?? string.Empty;
		_loginAttributeNormalized = _ldapOptions.Value.LoginAttribute?.Trim()?.ToLower() ?? string.Empty;
		_emailAttributeNormalized = _ldapOptions.Value.EmailAttribute?.Trim()?.ToLower() ?? string.Empty;
		_emailAttributeNormalizedV2 = _ldapOptions.Value.EmailAttributeV2?.Trim()?.ToLower() ?? string.Empty;
		_emailAttributeNormalizedV3 = _ldapOptions.Value.EmailAttributeV3?.Trim()?.ToLower() ?? string.Empty;
	}
	public List<LdapUserData> GetUsers()
	{
		var pageSize = 250;
		var server = new LdapDirectoryIdentifier(_ldapOptions.Value.Server, _ldapOptions.Value.ServerPort, true, false);
		var credits = new NetworkCredential($"{_ldapOptions.Value.ServiceAccount.Username}@{_ldapOptions.Value.DomainName}", _ldapOptions.Value.ServiceAccount.Password);
		
		using var ldapConnection = new LdapConnection(server, credits) {
			AuthType = AuthType.Basic
		};
		ldapConnection.SessionOptions.ReferralChasing = ReferralChasingOptions.None;
		ldapConnection.SessionOptions.ProtocolVersion = 3;
		ldapConnection.Bind();

		List<SearchResultEntry> searchResponseResult = new();
		
		PageResultRequestControl pageRequestControl = new PageResultRequestControl(pageSize);
		SearchOptionsControl searchOptionsControl = new SearchOptionsControl(SearchOption.DomainScope);

		var searchRequest = new SearchRequest(
			_ldapOptions.Value.BaseLdapFilter, 
			$"({_ldapOptions.Value.AdditionalLdapFilter})", 
			SearchScope.Subtree,
			new []{ "memberof", _displayNameAttributeNormalized, _loginAttributeNormalized,
				_emailAttributeNormalized, _emailAttributeNormalizedV2, _emailAttributeNormalizedV3
			});
		searchRequest.Controls.Add(pageRequestControl);
		searchRequest.Controls.Add(searchOptionsControl);
		
		var currPage = 0;
		while (true)
		{
			currPage++;
			if(ldapConnection.SendRequest(searchRequest) is not SearchResponse searchResponse)
				throw new Exception("Invalid search response");
    
			foreach (SearchResultEntry searchResponseEntry in searchResponse.Entries)
				searchResponseResult.Add(searchResponseEntry);
    
			foreach(var control in searchResponse.Controls)
			{
				if (control is PageResultResponseControl)
				{
					pageRequestControl.Cookie = ((PageResultResponseControl) control).Cookie;
					break;
				}
			}

			if (pageRequestControl.Cookie.Length == 0)break;
		}

		return searchResponseResult.Select(x => Map(x)).ToList();
	}

	private LdapUserData Map(SearchResultEntry entry)
	{
		var result = new LdapUserData();
		result.UserDistinguishedName = entry.DistinguishedName;

		foreach (DictionaryEntry item in entry.Attributes) {
			if (item.Value is not DirectoryAttribute attribute) continue;
			string[] values = attribute.GetValues(typeof(string)).Cast<string>().ToArray();
			if (values.Length == 0) continue;
			var attrName = attribute.Name.ToLower();
			if (attrName == "memberof")
				result.MemberOfGroups.AddRange(values.Select(x => LdapUtils.GetFriendlyName(x)));
			else if (attrName == _loginAttributeNormalized) 
				result.UserId = values[0];
			else if (attrName == _emailAttributeNormalizedV3)
				result.UserEmail = values[0];
			else if (attrName == _emailAttributeNormalizedV2)
				result.UserEmail = values[0];
			else if (attrName == _emailAttributeNormalized)
				result.UserEmail = values[0];
			else if (attrName == _displayNameAttributeNormalized)
				result.UserDisplayName = values[0];
		}
		return result;
	}
}