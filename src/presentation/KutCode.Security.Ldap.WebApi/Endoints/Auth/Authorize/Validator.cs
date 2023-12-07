
using FastEndpoints;
using FluentValidation;

namespace KutCode.Security.Ldap.WebApi.Endoints.Auth.Authorize;

public sealed class Validator : Validator<Request>
{
	public Validator()
	{
		RuleFor(x => x.Login).NotEmpty();
		RuleFor(x => x.Password).NotEmpty();
	}
}