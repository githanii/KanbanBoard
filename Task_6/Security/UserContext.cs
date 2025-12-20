using Application.Abstraction;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.Security
{
    public class UserContext: IUserContext
    {
        private readonly IHttpContextAccessor _http;

        public UserContext(IHttpContextAccessor http)
        {
            _http = http;
        }

        public bool IsAuthenticated()
            => _http.HttpContext?.User?.Identity?.IsAuthenticated == true;

        public int GetCurrentUserId()
        {
            var sub = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? _http.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return int.TryParse(sub, out var id) ? id : 0;
        }

        public string? GetCurrentUserName()
            => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        public string? GetCurrentUserRole()
            => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

        public bool IsInRole(string role)

            => string.Equals(GetCurrentUserRole(), role, StringComparison.OrdinalIgnoreCase);

    }
}
