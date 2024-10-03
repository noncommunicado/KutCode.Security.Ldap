using FluentAssertions;
using KutCode.Security.Ldap.WebApi.Services.Ldap;
using Ldap.Tests.Mocks;

namespace Ldap.Tests;

[TestFixture]
public class LdapGetUsersTests
{
	private readonly LdapGroupSearchService _ldap;
	public LdapGetUsersTests()
	{
		_ldap = LdapServiceMock.GetLdapGroupSearchServiceMock();
	}

	[Test]
	public async Task UsersLoad_Success()
	{
		var users = _ldap.GetUsers();
		users.Should().NotBeEmpty();
		users.Should().HaveCountGreaterThan(1);
	}
}