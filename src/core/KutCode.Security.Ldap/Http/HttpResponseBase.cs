
namespace KutCode.Security.Ldap.Http;

public sealed class HttpResponseBase<T>
{
	public HttpResponseBase(T value, string status = "OK", int code = 200)
	{
		Value = value;
		Status = status;
		Code = code;
	}
	public string Status { get; set; }
	public int Code { get; set; }
	public T Value { get; set; }

	public static HttpResponseBase<T> FromOK(T item) => new(item);
}