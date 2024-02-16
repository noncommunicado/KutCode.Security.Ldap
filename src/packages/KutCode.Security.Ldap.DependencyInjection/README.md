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