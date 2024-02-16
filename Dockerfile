FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
COPY . /src
WORKDIR /src

RUN dotnet restore
RUN dotnet publish "./src/presentation/KutCode.Security.Ldap.WebApi/KutCode.Security.Ldap.WebApi.csproj" -c Release -o ./publish --no-restore
# --------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /src/publish /app

VOLUME ./appsettings ./app/appsettings
VOLUME ./logs ./app/logs

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "/app/KutCode.Security.Ldap.WebApi.dll"]