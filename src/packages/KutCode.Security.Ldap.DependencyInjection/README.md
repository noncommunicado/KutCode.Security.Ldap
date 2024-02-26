# 📦 KutCode.Security.Ldap.DependencyInjection

Main package for connection with `KutCode.Security.Ldap.WebApi`  
(API microservice for LDAP auth)


[See more info here](https://github.com/hamaronooo/KutCode.Security.Ldap)

## ❓ Usage

After API deploy, use `DependencyInjection` to inject LDAP API repository.  
Example usage:
```csharp
using KutCode.Security.Ldap.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.GetRequiredSection("Ldap");
builder.Services
	.AddKutCodeLdapRepository(config, ServiceLifetime.Scoped);
```

appsettings.json example: 
```json
{
  "LdapSecurity": {
    "BaseUrl": "http://localhost:9080",
    "RpcBaseUrl": "http://localhost:11080"
  }
}
```

## gRPC 
If you want ot use gRPC to call Ldap security api:
```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapKutCodeLdapRpc(new LdapRepositoryInjectionConfiguration {
    BaseUrl = "http://localhost:9080",
    RpcBaseUrl = "http://localhost:9081"
    // You can use the same port for gRPC only if connection is TLS secured!
    // That's because HTTPv2 and HTTPv1 on the same port require TLS.
});
	
app.Run();
```
And than in any place of your code use: 
```csharp
var response = await new LdapAuthCommand {
		Login = "test", Password = "test"
	}.RemoteExecuteAsync();
```