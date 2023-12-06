using KutCode.Cve.Api.Configuration.Models;

namespace KutCode.Cve.Api.Configuration;

public static class CorsConfiguration
{
	public const string CorsPolicyName = "HrApiCorsPolicy";

	public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
	{
		var corsConfig = builder.Configuration.GetRequiredSection("Cors").Get<CorsConfigDto>()!;
		builder.Services.AddCors(options => {
			options.AddPolicy(CorsPolicyName, corsPolicyBuilder => {
				corsPolicyBuilder
					.SetIsOriginAllowed(x => {
						var url = new Uri(x);
						return corsConfig.Origins.Contains(url.Host);
					})
					.AllowAnyHeader()
					.WithExposedHeaders() // allows server send specified headers when CORS
					.AllowAnyMethod()
					.AllowCredentials();
			});
		});
		return builder;
	}
}