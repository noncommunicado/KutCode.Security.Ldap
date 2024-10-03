using FastEndpoints;
using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Models;
using KutCode.Security.Ldap.WebApi.Services.Ldap;

namespace KutCode.Security.Ldap.WebApi.Endoints.Ldap.Objects.List;

public sealed class Endpoint : EndpointWithoutRequest<HttpResponseBase<List<LdapUserData>>>
{
	public LdapGroupSearchService LdapService { get; set; }
	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Get("objects/users");
		Summary(s => {
			s.Summary = "Get all domain users with their groups";
			s.Description = "Depends on domain size operation may take a lot of time";
		});
	}

	public override Task<HttpResponseBase<List<LdapUserData>>> ExecuteAsync(CancellationToken ct)
	{
		ThrowIfAnyErrors();
		var response = LdapService.GetUsers();
		return Task.FromResult(HttpResponseBase<List<LdapUserData>>.FromOK(response));
	}
}