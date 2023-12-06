using FastEndpoints.Swagger;

namespace KutCode.Security.Ldap.WebApi.Configuration;

public static class SwaggerConfiguration
{
	public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
	{
		// var securitySettings = builder.Configuration
		// 	.GetSection("Security")
		// 	.Get<ApiSecuritySettings>();

		builder.Services.SwaggerDocument(o => {
			o.RemoveEmptyRequestSchema = true;
			o.MaxEndpointVersion = 2;
			o.MinEndpointVersion = 1;
			o.ShortSchemaNames = true;
			o.DocumentSettings = s => {
				s.Title = "Some api description";
				s.Version = "v1";
			};

			o.TagDescriptions = t => {
				//t["Some"] = "Some controller name";
			};

			// if (securitySettings?.Static is not null && securitySettings.Static.IsEnabled && !string.IsNullOrEmpty(securitySettings.Static.HeaderName))
			// {
			// 	o.DocumentSettings = s => {
			// 		s.AddAuth(AuthenticationsSchemes.StaticKeyAuthScheme, new OpenApiSecurityScheme {
			// 			Name = securitySettings.Static.HeaderName,
			// 			In = OpenApiSecurityApiKeyLocation.Header,
			// 			Type = OpenApiSecuritySchemeType.ApiKey,
			// 			Description = "Статический токен для авторизации"
			// 		});
			// 	};
			// }
		});


		return builder;
	}
}