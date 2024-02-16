using FastEndpoints;
using KutCode.Security.Ldap.Http;
using Serilog;

namespace KutCode.Security.Ldap.WebApi.Endoints.Auth.Authorize;

public sealed class Endpoint : Endpoint<LdapLoginRequest, HttpResponseBase<LdapAuthenticationResponse>>
{
	public LdapService LdapService { get; set; }
	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Post("auth");
	}

	public override async Task<HttpResponseBase<LdapAuthenticationResponse>> ExecuteAsync(LdapLoginRequest req, CancellationToken ct)
	{
		ThrowIfAnyErrors();
		Log.Information("Sending LDAP authorization for {User}", req.Login);
		var response = await Task.Run(() => LdapService.Authenticate(req.Login, req.Password), ct);
		return HttpResponseBase<LdapAuthenticationResponse>.FromOK(response);
	}
}