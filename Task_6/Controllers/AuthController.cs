using Application.Abstraction;
using Application.DTOs;
using Application.Presistence;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _users;
        private readonly IRefreshToken _refreshTokens;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokens;

        public AuthController(
            IUserRepository users,
            IRefreshToken refreshTokens,
            IPasswordHasher hasher,
            ITokenService tokens)
        {
            _users = users;
            _refreshTokens = refreshTokens;
            _hasher = hasher;
            _tokens = tokens;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var user = await _users.GetByUsernameAsync(dto.UserName);
            if (user == null)
                return Unauthorized("Invalid credentials");

            if (!_hasher.VerifyPassword(user.PasswordHash, dto.Password))
                return Unauthorized("Invalid credentials");

            var access = _tokens.GenerateAccessToken(user.Id, user.Role, user.UserName);

            var refresh = _tokens.GenerateRefreshToken();
            var refreshHash = _tokens.GetSha256Hash(refresh);

            await _refreshTokens.RemoveUserTokensAsync(user.Id);

            await _refreshTokens.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new AuthResponseDto
            {
                AccessToken = access,
                RefreshToken = refresh
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh( string refreshToken)
        {
            var hash = _tokens.GetSha256Hash(refreshToken);
            var stored = await _refreshTokens.GetByTokenAsync(hash);

            if (stored == null || !stored.IsActive)
                return Unauthorized("Invalid refresh token");

            var user = await _users.GetByUserIdAsync(stored.UserId);
            if (user == null)
                return Unauthorized();

            await _refreshTokens.RemoveUserTokensAsync(user.Id);

            var newRefresh = _tokens.GenerateRefreshToken();
            var newRefreshHash = _tokens.GetSha256Hash(newRefresh);

            await _refreshTokens.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                TokenHash = newRefreshHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            var newAccess = _tokens.GenerateAccessToken(user.Id, user.Role, user.UserName);

            return Ok(new AuthResponseDto
            {
                AccessToken = newAccess,
                RefreshToken = newRefresh
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("All fields are required.");

            if (dto.Password.Length < 6)
                return BadRequest("Password must be at least 6 characters.");

            if (await _users.GetByUsernameAsync(dto.UserName) != null)
                return Conflict("Username already exists.");

            if (await _users.GetByEmailAsync(dto.Email) != null)
                return Conflict("Email already exists.");

            var user = new User
            {
                UserName = dto.UserName.Trim(),
                Email = dto.Email.Trim(),
                Role = "User",
                PasswordHash = _hasher.HashPassword(dto.Password)
            };

            await _users.AddAsync(user);

            var access = _tokens.GenerateAccessToken(user.Id, user.Role, user.UserName);

            var refresh = _tokens.GenerateRefreshToken();
            var refreshHash = _tokens.GetSha256Hash(refresh);

            await _refreshTokens.RemoveUserTokensAsync(user.Id);

            await _refreshTokens.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new AuthResponseDto
            {
                AccessToken = access,
                RefreshToken = refresh
            });
        }
    }
}
