using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Presistence
{
    public interface IBoardRepository
    {
        Task <List<Board>> GetAllBoardsAsync();
        Task<Board?> GetBoardByIdAsync(int boardId);
        Task<Board> AddBoardAsync(Board board);
        Task<int> UpdateBoardAsync(Board board, int Id);
        Task<int> DeleteBoardAsync(int Id);
        Task<List<Board>> GetAllByOwnerAsync(int ownerId);
        Task<Board?> GetByIdForOwnerAsync(int boardId, int ownerId);

    }
}
