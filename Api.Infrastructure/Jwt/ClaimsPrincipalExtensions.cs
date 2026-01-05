using System.Security.Claims;

namespace Api.Infrastructure.Jwt
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetBranch(this ClaimsPrincipal principal)
            => principal.FindFirstValue(FSHClaims.BranchId);
        private static string? FindFirstValue(this ClaimsPrincipal principal, string claimType) =>
        principal is null
            ? throw new ArgumentNullException(nameof(principal))
            : principal.FindFirst(claimType)?.Value;

    }
}
