using FastEndpoints.Swagger;

namespace KutCode.Cve.Api.Configuration;

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
				s.Title = "API HR сервиса упрощения подбора";
				s.Version = "v1";
			};

			o.TagDescriptions = t => {
				t["Cve"] = "Операции с CVE";
				t["Queue"] = "Очереди обработчиков";
				t["Report"] = "Отчеты";
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