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
RUN apt-get update && apt-get install --upgrade -y libldap-2.5.0 && ln -s /usr/lib/x86_64-linux-gnu/libldap.so.2 /usr/lib/libldap-2.5.so.0

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "/app/KutCode.Security.Ldap.WebApi.dll"]