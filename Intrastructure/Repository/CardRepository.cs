using Application.Presistence;
using Domain.Entities;
using Intrastructure.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBoards.Domain.Entities;

namespace Intrastructure.Repository
{
    public class CardRepository: ICardRepository
    {
        private readonly AppDbContext _context;
        public CardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Card card)
        {
            try
            {
                await _context.Cards.AddAsync(card);
                return await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int Id)
        {
            try
            {
                return await _context.Cards
                    .Where(l => l.Id == Id)
                    .ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<Card>> GetAllAsync()
        {
            return await _context.Cards
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Card>> GetAllByListAsync(int ListId, bool IsDeleted)
        {
            return await _context.Cards
                .AsNoTracking()
                .Where(c => c.ListId == ListId && c.IsDeleted == IsDeleted)
                .OrderBy(c => c.OrderIndex)
                .ToListAsync();
        }

        public  async Task<IEnumerable<Card>> GetAllByListAsync(int ListId)
        {
            return await _context.Cards
                  .AsNoTracking()
                  .Where(c => c.ListId == ListId && !c.IsDeleted) 
                  .OrderBy(c => c.OrderIndex)
                  .ToListAsync();
        
        }

        public async Task<Card?> GetByIdAsync(int Id)
        {
            return await _context.Cards.
                FirstOrDefaultAsync (c => c.Id == Id);

        }

        public async Task<int> GetMaxOrderIndexAsync(int listId)
        {
            var max = await _context.Cards
                  .Where(c => c.ListId == listId && !c.IsDeleted)
                  .Select(c => (int?)c.OrderIndex)
                  .MaxAsync();

            return max ?? -1;
        }

        public async Task ReorderOnMoveAsync( int CardId, int targetListId, int targetOrderIndex)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == CardId);
            if (card == null) return;

            var oldIndex = card.OrderIndex;

            await _context.Cards
                .Where(c =>  c.Id != CardId && c.OrderIndex > oldIndex && !c.IsDeleted)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.OrderIndex, x => x.OrderIndex - 1));

            await _context.Cards
                .Where(c => c.ListId == targetListId && c.OrderIndex >= targetOrderIndex && !c.IsDeleted)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.OrderIndex, x => x.OrderIndex + 1));

            await _context.Cards
                .Where(c => c.Id == CardId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.ListId, targetListId)
                    .SetProperty(x => x.OrderIndex, targetOrderIndex));
        }

        public async Task<IEnumerable<Card>> SearchAsync(int boardId, string Title)
        {
            return await _context.Cards
                .Where(c => c.List.BoardId == boardId && c.Title.Contains(Title))
                .ToListAsync(); 
        }

        public async Task<int> Update(Card card, int Id)
        {
            _context.Cards.Update(card);
                
               
            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> UpdateOrderIndexAsync(int cardId, int newIndex)
        {
return await _context.Cards
                .Where(c => c.Id == cardId)
                .ExecuteUpdateAsync(c => c.SetProperty(c => c.OrderIndex, newIndex));
        }
    }
}
