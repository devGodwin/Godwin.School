using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Finance.Management.Api.Configuration;
using Finance.Management.Api.Model.Response;
using Finance.Management.Api.Model.ResponseModel;
using Microsoft.IdentityModel.Tokens;

namespace Finance.Management.Api.Helper;

public static class TokenGenerator
{
    public static TokenResponse GenerateToken(this StatementOfAccountResponseModel statementOfAccount, BearerTokenConfig config)
    {
        if (statementOfAccount is null)
            throw new ArgumentNullException(nameof(statementOfAccount),"Statement of account must not be null or empty");
        
        if (config is null)
            throw new ArgumentNullException(nameof(config),"Bearer token configuration must not be null or empty");

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.Key));
        var now = DateTime.UtcNow;

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Role, statementOfAccount.AdminName)
        };

        var token = new JwtSecurityToken(
           
           config.Issuer,
           config.Audience,
           claims,
           now.AddMilliseconds(0),
           now.AddHours(12),
           new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature)
        );

        string tokenString = tokenHandler.WriteToken(token);

        return new TokenResponse()
        {
            BearerToken = tokenString,
            Expiry = token.Payload.Exp
        };
    }
}