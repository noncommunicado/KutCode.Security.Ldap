using FastEndpoints;
using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Rpc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapRemote("http://ldap.nginx3.neftm.local:9080", c => {
	c.Register<LdapAuthCommand, LdapAuthenticationResponse>();
});

Task.Run(async () => {
	await Task.Delay(100);
	var response = await new LdapAuthCommand {
		Login = "test", Password = "test"
	}.RemoteExecuteAsync();
	Console.Out.WriteLine(response.Authorized);
	Console.Out.WriteLine(response.UserData?.UserDisplayName);
});
	
app.Run();