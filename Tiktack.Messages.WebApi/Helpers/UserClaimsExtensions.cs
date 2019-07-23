using System.Security.Claims;

namespace Tiktack.Messaging.WebApi.Helpers
{
    public static class UserClaimsExtensions
    {
        public static string GetNameIdentifier(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
