using Application.Abstraction;
using Application.Presistence;
using Domain.Entities;
using TeamBoards.Domain.Entities;

namespace Application.Services
{
    public class CardService
    {
        private readonly ICardRepository _cards;
        private readonly IListRepository _lists;
        private readonly IBoardRepository _boards;
        private readonly IUserContext _user;

        public CardService(ICardRepository cards, IListRepository lists, IBoardRepository boards, IUserContext user)
        {
            _cards = cards;
            _lists = lists;
            _boards = boards;
            _user = user;
        }

        public async Task<Card?> CreateAsync(int listId, string title, string? description)
        {
            var list = await _lists.GetByIdAsync(listId);
            if (list == null) return null;

            var board = await _boards.GetBoardByIdAsync(list.BoardId);
            if (board == null || board.OwnerId != _user.GetCurrentUserId())
                return null;

            var max = await _cards.GetMaxOrderIndexAsync(listId);

            var card = new Card
            {
                ListId = listId,
                Title = title,
                Description = description,
                OrderIndex = max + 1,
                CreatedAt = DateTime.UtcNow,
                CreatedById = _user.GetCurrentUserId(),
                IsDeleted = false
            };

            await _cards.AddAsync(card);
            return card;
        }

        public async Task<bool> UpdateAsync(int cardId, string title, string? description)
        {
            var card = await _cards.GetByIdAsync(cardId);
            if (card == null) return false;

            var list = await _lists.GetByIdAsync(card.ListId);
            if (list == null) return false;

            var board = await _boards.GetBoardByIdAsync(list.BoardId);
            if (board == null || board.OwnerId != _user.GetCurrentUserId())
                return false;

            card.Title = title;
            card.Description = description;

            await _cards.Update(card, cardId);
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int cardId)
        {
            var card = await _cards.GetByIdAsync(cardId);
            if (card == null) return false;

            var list = await _lists.GetByIdAsync(card.ListId);
            if (list == null) return false;

            var board = await _boards.GetBoardByIdAsync(list.BoardId);
            if (board == null || board.OwnerId != _user.GetCurrentUserId())
                return false;

            card.IsDeleted = true;
            await _cards.Update(card, cardId);
            return true;
        }

        public async Task<bool> MoveAsync(int cardId, int targetListId, int targetOrderIndex)
        {
            var card = await _cards.GetByIdAsync(cardId);
            if (card == null) return false;

            var sourceList = await _lists.GetByIdAsync(card.ListId);
            if (sourceList == null) return false;

            var board = await _boards.GetBoardByIdAsync(sourceList.BoardId);
            if (board == null || board.OwnerId != _user.GetCurrentUserId())
                return false;

            await _cards.ReorderOnMoveAsync(cardId, targetListId, targetOrderIndex);
            return true;
        }
    }
}
