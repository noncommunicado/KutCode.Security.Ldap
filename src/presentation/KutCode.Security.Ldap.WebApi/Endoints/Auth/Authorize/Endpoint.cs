using FastEndpoints;
using KutCode.Security.Ldap.WebApi.Http;
using KutCode.Security.Ldap.WebApi.Ldap;

namespace KutCode.Security.Ldap.WebApi.Endoints.Auth.Authorize;

public sealed class Endpoint : Endpoint<Request, HttpResponseBase<LdapAuthentication>>
{
	public LdapService LdapService { get; set; }
	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Post("auth");
	}

	public override async Task<HttpResponseBase<LdapAuthentication>> ExecuteAsync(Request req, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		var response = await Task.Run(() => LdapService.Authenticate(req.Login, req.Password), ct);
		return HttpResponseBase<LdapAuthentication>.FromOK(response);
	}
}