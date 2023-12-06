using System.Globalization;
using FastEndpoints;
using FluentValidation;

namespace KutCode.Security.Ldap.WebApi.Configuration;

public static class FastEndpointsConfiguration
{
	internal static WebApplicationBuilder AddFastEndpoints(this WebApplicationBuilder webBuilder)
	{
		webBuilder.Services.AddFastEndpoints(opts => { opts.IncludeAbstractValidators = true; });

		// Set fluent validation error language
		var configCulture = webBuilder.Configuration.GetSection("Culture").Get<string>();
		ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(configCulture ?? "en-US");

		webBuilder.Services.AddAuthentication();
		webBuilder.Services.AddAuthorization();
		
		return webBuilder;
	}
}