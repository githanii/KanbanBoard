using Domain.Entities;
using Intrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Presistence;

namespace Infrastructure.Repository
{
    public class RefreshTokenRepository : IRefreshToken
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<RefreshToken?> GetByTokenAsync(string tokenHash)
            => _context.RefreshTokens.AsNoTracking()
                .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserTokensAsync(int userId)
        {
            await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
