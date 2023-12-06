using System.Globalization;
using FluentValidation;

namespace KutCode.Cve.Api.Configuration;

public static class FastEndpointsConfiguration
{
	internal static WebApplicationBuilder AddFastEndpoints(this WebApplicationBuilder webBuilder)
	{
		webBuilder.Services.AddFastEndpoints(opts => { opts.IncludeAbstractValidators = true; });

		// Set fluent validation error language to russian
		ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ru");

		return webBuilder;
	}
}