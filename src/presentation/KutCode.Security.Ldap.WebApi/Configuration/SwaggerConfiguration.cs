using FastEndpoints.Swagger;

namespace KutCode.Security.Ldap.WebApi.Configuration;

public static class SwaggerConfiguration
{
	public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
	{
		builder.Services.SwaggerDocument(o => {
			o.RemoveEmptyRequestSchema = true;
			o.MaxEndpointVersion = 2;
			o.MinEndpointVersion = 1;
			o.ShortSchemaNames = true;
			o.DocumentSettings = s => {
				s.Title = "KutCode Ldap API description";
				s.Version = "v1";
			};

			o.TagDescriptions = t => {
				t["Auth"] = "Authorization endpoints";
				t["Ping"] = "App availability testing";
				t["Objects"] = "Interact with other telegram objects";
			};
		});
		return builder;
	}
}