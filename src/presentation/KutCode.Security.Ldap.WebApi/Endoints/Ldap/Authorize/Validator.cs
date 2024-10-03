using FastEndpoints;
using FluentValidation;
using KutCode.Security.Ldap.Http;

namespace KutCode.Security.Ldap.WebApi.Endoints.Ldap.Authorize;

public sealed class Validator : Validator<LdapLoginRequest>
{
	public Validator()
	{
		RuleFor(x => x.Login).NotEmpty();
		RuleFor(x => x.Password).NotEmpty();
	}
}