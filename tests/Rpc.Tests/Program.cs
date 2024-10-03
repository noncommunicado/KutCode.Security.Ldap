using FastEndpoints;
using KutCode.Security.Ldap.DependencyInjection;
using KutCode.Security.Ldap.Rpc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapKutCodeLdapRpc(app.Configuration["LdapUrl"]);
Task.Run(async () => {
	await Task.Delay(300);
	var response = await new LdapAuthCommand(
			app.Configuration["LdapUser"], app.Configuration["LdapPassword"])
		.RemoteExecuteAsync();
	Console.Out.WriteLine(response.Authorized);
	Console.Out.WriteLine(response.UserData?.UserDisplayName);
});

Task.Run(async () => {
	await Task.Delay(300);
	var response = await new LdapGetUserListCommand()
		.RemoteExecuteAsync();
	Console.Out.WriteLine(response.Count);
	Console.Out.WriteLine(response.Take(10));
});

await app.RunAsync();