# 📦 KutCode.Security.Ldap.DependencyInjection

Main package for connection with `KutCode.Security.Ldap.WebApi`  
(API microservice for LDAP auth)


[See more info here](https://github.com/hamaronooo/KutCode.Security.Ldap)

## ❓ Usage

After API deploy, use `DependencyInjection` to inject LDAP API repository.   

🛠️ Configuring:
```csharp
using KutCode.Security.Ldap.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.GetRequiredSection("Ldap");
builder.Services
	.AddKutCodeLdapRepository(config, ServiceLifetime.Scoped);
```

🔍 `appsettings.json` example: 
```json
{
  "LdapSecurity": {
    "BaseUrl": "http://localhost:9080",
    "RpcBaseUrl": "http://localhost:11080"
  }
}
```
🏃‍♂️ Usage:
```csharp
class Worker {
    private readonly IKutCodeLdapRepository _ldap;
    public Worker(IKutCodeLdapRepository ldap) {
        _ldap = ldap;
    }
    
    async Task AuthAsync(CancellationToken ct = default) {
        var authResponse = await _ldap.LoginAsync(default);
    }
}
```

## ✨ RPC 
If you want ot use `gRPC` to call Ldap security API just configure `WebApplication`.  
⚠️ Notice!  
You can use the same port for `gRPC` only if connection is `TLS` secured!  
That's because `HTTPv2` and `HTTPv1` listen on the same port requires `TLS`.  
```csharp
var builder = WebApplication.CreateBuilder(args);
// ...
var app = builder.Build();

// Just add this
app.MapKutCodeLdapRpc();
// OR you can rewrite RpcBaseUrl from appsettings
app.MapKutCodeLdapRpc("http://ldap.domain.local:9081");
	
app.Run();
```
And than in *any* place of your code use: 
```csharp
LdapAuthCommand command = new(login: "test", password: "test");
var response = await command.RemoteExecuteAsync();
```