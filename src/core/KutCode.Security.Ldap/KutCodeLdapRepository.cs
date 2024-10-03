using System.Net.Http.Json;
using KutCode.Security.Ldap.Http;
using KutCode.Security.Ldap.Interfaces;
using KutCode.Security.Ldap.Models;

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
	
	
	public async Task<HttpResponseBase<List<LdapUserData>>> GetUsersAsync(CancellationToken ct = default)
	{
		var message = new HttpRequestMessage(HttpMethod.Get, "/api/v1/objects/users");
		HttpResponseMessage responseRaw = await Client.SendAsync(message, ct);
		if (responseRaw.IsSuccessStatusCode is false) {
			var responseTextMessage = await responseRaw.Content.ReadAsStringAsync(ct);
			throw new HttpRequestException(responseTextMessage, null, responseRaw.StatusCode);
		}
		return await responseRaw.Content.ReadFromJsonAsync<HttpResponseBase<List<LdapUserData>>>(ct);
	}
}