# 📦 KutCode.Security.Ldap

Main package for connection with `KutCode.Security.Ldap.WebApi`  
(API microservice for LDAP auth)


[See more info here](https://github.com/hamaronooo/KutCode.Security.Ldap)

## ❓ Usage

After API deploy, use `KutCodeLdapRepository` class to make client requests.  
Example usage:
```csharp
using KutCode.Security.Ldap;
using KutCode.Security.Ldap.Http;

HttpClient httpClient = new () {
	BaseAddress = new Uri("http://localhost:9090")
};

KutCodeLdapRepository client = new (httpClient);

LdapLoginRequest request = new("login", "password");
var response = await client.LoginAsync(request);

Console.WriteLine($"Is Authorized: {response.Authorized}");
Console.WriteLine($"My name is: {response.UserData.Value?.UserDisplayName}");
```