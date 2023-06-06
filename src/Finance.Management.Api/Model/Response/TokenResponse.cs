namespace Finance.Management.Api.Model.Response;

public sealed class TokenResponse
{
    public string BearerToken { get; set; }
    public int? Expiry { get; set; }
}