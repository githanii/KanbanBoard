using Application.Abstraction;
using Application.Presistence;
using Domain.Entities;
using Intrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class BoardRepository : IBoardRepository
    {
        private readonly AppDbContext _Context;

        public BoardRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        public async Task<Board> AddBoardAsync(Board board)
        {
            await _Context.Boards.AddAsync(board);
            await _Context.SaveChangesAsync();
            return board;
        }

        public async Task<int> DeleteBoardAsync(int Id)
        {
            return await _Context.Boards
                .Where(b => b.Id == Id)
                .ExecuteDeleteAsync();
        }

        public async Task<List<Board>> GetAllBoardsAsync()
        {
            return await _Context.Boards.ToListAsync();
        }

        public Task<Board?> GetBoardByIdAsync(int boardId)
        {
            return _Context.Boards.AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == boardId);
        }

        public async Task<int> UpdateBoardAsync(Board board, int id)
        {
            return await _Context.Boards
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(b => b
                    .SetProperty(x => x.Name, board.Name)
                    .SetProperty(x => x.OwnerId, board.OwnerId)
                );
        }
        public async Task<List<Board>> GetAllByOwnerAsync(int ownerId)
        {
            return await _Context.Boards
                .AsNoTracking()
                .Where(b => b.OwnerId == ownerId)
                .ToListAsync();
        }

        public async Task<Board?> GetByIdForOwnerAsync(int boardId, int ownerId)
        {
            return await _Context.Boards
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == boardId && b.OwnerId == ownerId);
        }


    }

}
