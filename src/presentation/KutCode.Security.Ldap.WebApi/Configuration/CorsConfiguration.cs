using KutCode.Security.Ldap.WebApi.Configuration.Models;

namespace KutCode.Security.Ldap.WebApi.Configuration;

public static class CorsConfiguration
{
	public const string CorsPolicyName = "ApiCorsPolicy";

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