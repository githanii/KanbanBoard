using Application.Abstraction;
using Application.Presistence;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BoardRepoServices
    {
        private readonly IBoardRepository _repository;

        public BoardRepoServices(IBoardRepository repository)
        {
            _repository = repository;
        }

        public Task<Board> AddBoardAsync(Board board) =>
            _repository.AddBoardAsync(board);

        public Task<int> DeleteBoardAsync(int id) =>
            _repository.DeleteBoardAsync(id);

        public Task<List<Board>> GetAllBoardsAsync() =>
            _repository.GetAllBoardsAsync();

     

        public Task<int> UpdateBoardAsync(Board board, int id) =>
            _repository.UpdateBoardAsync(board, id);

        public Task<List<Board>> GetMyBoardsAsync(int ownerId) =>
        _repository.GetAllByOwnerAsync(ownerId);

        public Task<Board?> GetMyBoardByIdAsync(int boardId, int ownerId) =>
            _repository.GetByIdForOwnerAsync(boardId, ownerId);

     
    }
}
