
using FastEndpoints;

namespace KutCode.Security.Ldap.WebApi.Endoints.Ping;

public sealed class Endpoint : EndpointWithoutRequest
{
	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Get("ping");
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
		await SendOkAsync("OK", ct);
	}
}