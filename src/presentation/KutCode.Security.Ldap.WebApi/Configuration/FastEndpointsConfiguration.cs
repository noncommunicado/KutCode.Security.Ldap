using System.Globalization;
using System.Net;
using FastEndpoints;
using FluentValidation;
using KutCode.Security.Ldap.WebApi.Configuration.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace KutCode.Security.Ldap.WebApi.Configuration;

public static class FastEndpointsConfiguration
{
	internal static WebApplicationBuilder AddFastEndpoints(this WebApplicationBuilder webBuilder)
	{
		webBuilder.Services.AddFastEndpoints(opts => {
			opts.IncludeAbstractValidators = true;
		});

		// Set fluent validation error language
		var configCulture = webBuilder.Configuration.GetSection("Culture").Get<string>();
		ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(configCulture ?? "en-US");

		webBuilder.Services.AddAuthentication();
		webBuilder.Services.AddAuthorization();
		
		// gRpc calls support
		webBuilder.AddRpc();
		
		return webBuilder;
	}
	
	private static WebApplicationBuilder AddRpc(this WebApplicationBuilder bldr)
	{
		var config = bldr.Configuration.GetSection("Rpc").Get<RpcConfigDto>();
		var basePort = bldr.Configuration.GetSection("ListenPort").Get<int?>() ?? 80;
		if (config is null || config.Enabled == false) return bldr;
		
		bldr.Services.AddSingleton(config);
		bldr.AddHandlerServer();
		bldr.WebHost.ConfigureKestrel(o => {
			if (config.Secure) {
				o.ConfigureEndpointDefaults(x => x.Protocols = HttpProtocols.Http1AndHttp2);
			}
			else {
				o.Listen(IPAddress.Any, basePort, opts => opts.Protocols = HttpProtocols.Http1);
				o.Listen(IPAddress.Any, config.Port, opts => opts.Protocols = HttpProtocols.Http2);
			}
		});

		bldr.Services.AddGrpcReflection();
		return bldr;
	}
}