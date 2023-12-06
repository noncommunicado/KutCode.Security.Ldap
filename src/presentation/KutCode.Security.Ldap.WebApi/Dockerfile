FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /publish
# --------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=build /publish /app
EXPOSE 80
ENTRYPOINT ["dotnet", "/app/Neftm.FuelGeoPrice.Api.dll"]