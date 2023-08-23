using APIs.ViewModel.Others;
using System.Security.Claims;

namespace APIs.Contract
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromxpiredToken(string token);
        ClaimVM ExtractClaimsFromJwt(string token);
    }
}
