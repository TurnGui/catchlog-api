using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CatchLog.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetAnglerId(this ClaimsPrincipal user)
    {
        var value = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (!int.TryParse(value, out var id))
        {
            throw new InvalidOperationException("Token does not contain a valid angler id.");
        }

        return id;
    }
}