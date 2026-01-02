 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBoards.Domain.Entities;

namespace Application.Presistence
{
    public interface ICardRepository
    {
        Task<Card?> GetByIdAsync(int Id);
        Task<IEnumerable<Card>> GetAllByListAsync(int ListId);
        Task<IEnumerable<Card>> GetAllAsync();

        Task<int> AddAsync(Card card);
        Task<int> Update(Card card, int Id);
        Task<int> Delete(int Id);

        Task<IEnumerable<Card>> SearchAsync(int boardId, string Title);

        Task ReorderOnMoveAsync( int CardId, int targetListId, int targetOrderIndex);
        Task<int> GetMaxOrderIndexAsync(int listId);
        Task<int> UpdateOrderIndexAsync(int cardId, int newIndex);


    }
}
