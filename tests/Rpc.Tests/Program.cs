using FastEndpoints;
using KutCode.Security.Ldap.DependencyInjection;
using KutCode.Security.Ldap.Rpc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapKutCodeLdapRpc("http://localhost:9081");
Task.Run(async () => {
	await Task.Delay(300);
	var response = await new LdapAuthCommand("kutumov.n", "Borrow1488")
		.RemoteExecuteAsync();
	Console.Out.WriteLine(response.Authorized);
	Console.Out.WriteLine(response.UserData?.UserDisplayName);
});
	
await app.RunAsync();