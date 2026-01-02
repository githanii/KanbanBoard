using Application.Abstraction;
using Application.DTOs;
using Application.Presistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ListService
    {
        private readonly IListRepository _lists;
        private readonly IBoardRepository _boards;
        private readonly IUserContext _user;

        public ListService(IListRepository lists, IBoardRepository boards, IUserContext user)
        {
            _lists = lists;
            _boards = boards;
            _user = user;
        }

        public async Task<Domain.Entities.List?> CreateAsync(int boardId, string title)
        {
            try
            {
                var board = await _boards.GetBoardByIdAsync(boardId);
                if (board == null || board.OwnerId != _user.GetCurrentUserId())
                    return null;

                var max = await _lists.GetMaxOrderIndexAsync(boardId);

                var list = new Domain.Entities.List
                {
                    BoardId = boardId,
                    Title = title,
                    OrderIndex = max + 1
                };

                await _lists.AddAsync(list);
                return list;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateAsync(int listId, string title, int orderIndex)
        {
            try {
                var list = await _lists.GetByIdAsync(listId);
                if (list == null) return false;

                var board = await _boards.GetBoardByIdAsync(list.BoardId);
                if (board == null || board.OwnerId != _user.GetCurrentUserId())
                    return false;

                list.Title = title;
                list.OrderIndex = orderIndex;

                await _lists.Update(list, listId);
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
                    }
            
            
            }

        public async Task<bool> DeleteAsync(int listId)
        {
            try {
                var list = await _lists.GetByIdAsync(listId);
                if (list == null) return false;

                var board = await _boards.GetBoardByIdAsync(list.BoardId);
                if (board == null || board.OwnerId != _user.GetCurrentUserId())
                    return false;

                await _lists.DeleteAsync(listId);
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            }

        public async Task<List<ListDto>> GetListsAsync()
        {
            try
            {
                var lists = await _lists.GetAllAsync();

                return lists.Select(l => new ListDto
                {
                    Id = l.Id,
                    BoardId = l.BoardId,
                    Title = l.Title,
                    OrderIndex = l.OrderIndex,
                    Cards = l.Cards.Select(c => new CardDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        OrderIndex = c.OrderIndex
                    }).ToList()
                }).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
