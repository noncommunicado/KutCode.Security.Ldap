using System.Net.Http.Json;
using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Interfaces;

namespace KutCode.Security.Ldap;

public class KutCodeLdapRepository : IKutCodeLdapRepository
{
	protected HttpClient Client { get; set; }
	protected KutCodeLdapRepository() {} 
	public KutCodeLdapRepository(HttpClient client) => Client = client;
	public async Task<LdapAuthenticationResponse> LoginAsync(LdapLoginRequest request, CancellationToken ct = default)
	{
		var message = new HttpRequestMessage(HttpMethod.Post, "/api/v1/auth");
		message.Content = JsonContent.Create(request);
		HttpResponseMessage responseRaw = await Client.SendAsync(message, ct);
		if (responseRaw.IsSuccessStatusCode is false)
			return new LdapAuthenticationResponse{Authorized = false};
		var responseModel = await responseRaw.Content.ReadFromJsonAsync<HttpResponseBase<LdapAuthenticationResponse>>(ct);
		return responseModel!.Value;
	}
}