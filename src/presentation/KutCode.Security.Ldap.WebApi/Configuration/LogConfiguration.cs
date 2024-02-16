using Serilog;
using Serilog.Events;

namespace KutCode.Security.Ldap.WebApi.Configuration;

internal static class LogConfiguration
{
	internal static WebApplicationBuilder ConfigureSerilogging(this WebApplicationBuilder builder)
	{
		builder.Logging.ClearProviders();
		builder.Host.UseSerilog((_, __, configuration) => {
			configuration
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.Enrich.FromLogContext();
	#if DEBUG
			configuration.WriteTo.Logger(c => c.WriteTo.Console(LogEventLevel.Information));
	#else
			configuration
				.WriteTo.File(Path.Combine("logs", "_.log"),
					rollingInterval: RollingInterval.Day,
					restrictedToMinimumLevel: LogEventLevel.Information,
					retainedFileCountLimit: 30);
	#endif
		});
		return builder;
	}
	
	internal static WebApplication UseHttpRequestsLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            // options.GetLevel = (_, _, _) => LogEventLevel.Warning;
            // Attach additional properties to the request completion event
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                var ipHeader = httpContext.Request.Headers.FirstOrDefault(x => x.Key.Equals("X-Real-IP"));
                var forwardedHeader = httpContext.Request.Headers.FirstOrDefault(x => x.Key.Equals("X-Forwarded-For"));

                diagnosticContext.Set("RemoteIpAddress", ipHeader.Value.Count > 0
                    ? ipHeader.Value[0]
                    : httpContext.Request.HttpContext.Connection.RemoteIpAddress);
                diagnosticContext.Set("Forwarded",
                    forwardedHeader.Value.Count > 0 ? forwardedHeader.Value[0] : "[none]");
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme.ToUpper());
                diagnosticContext.Set("ContentLength", httpContext.Request.ContentLength ?? 0);
                diagnosticContext.Set("ContentType", httpContext.Request.ContentType ?? "[none]");
                diagnosticContext.Set("HeadersCount", httpContext.Request.Headers.Count);
                diagnosticContext.Set("CookiesCount", httpContext.Request.Cookies.Count);
                diagnosticContext.Set("TraceIdentifier", httpContext.TraceIdentifier);
            };

            options.MessageTemplate = Environment.NewLine +
                                      "\t{RequestScheme} ({TraceIdentifier}) from {RemoteIpAddress} with {RequestMethod} {RequestPath}" +
                                      Environment.NewLine +
                                      "\tResponded {StatusCode} in {Elapsed:0.0000} ms; Body: {ContentLength} bytes; " +
                                      Environment.NewLine +
                                      "\tContentType: {ContentType}; Headers: {HeadersCount}; Cookies: {CookiesCount}; Forwarded: {Forwarded}.";
            options.IncludeQueryInRequestPath = true;
        });
        return app;
    }
}