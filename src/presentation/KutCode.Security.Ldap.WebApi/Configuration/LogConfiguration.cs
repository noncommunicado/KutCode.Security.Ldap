using Serilog;
using Serilog.Events;

namespace KutCode.Security.Ldap.WebApi.Configuration;

public static class LogConfiguration
{
	public static WebApplicationBuilder ConfigureSerilogging(this WebApplicationBuilder builder)
	{
		builder.Host.UseSerilog();

		var loggerConfig = new LoggerConfiguration()
			.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
			.Enrich.FromLogContext();

#if DEBUG
		loggerConfig.WriteTo.Logger(c => c.WriteTo.Console(LogEventLevel.Information));

#else
		loggerConfig.WriteTo.Logger(c => c.WriteTo.File(Path.Combine("logs", "_.log"),
				rollingInterval: RollingInterval.Day,
				restrictedToMinimumLevel: LogEventLevel.Information));
#endif

		Log.Logger = loggerConfig.CreateLogger();
		return builder;
	}
}