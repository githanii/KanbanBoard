using Application.Presistence;
using Domain.Entities;
using Intrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intrastructure.Repository
{
    public class ListRepository: IListRepository
    {
        private readonly AppDbContext _Context;
        public ListRepository(AppDbContext Context)
        {
            _Context = Context;
        }


        public async Task<int> AddAsync(List list)
        {
            await _Context.Lists.AddAsync(list);
            return await _Context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int Id)
        {
            return await _Context.Lists
                .Where(l => l.Id == Id)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<List>> GetAllByBoardAsync(int boardId)
        {
            return await _Context.Lists
                .AsNoTracking()
                .Where(l => l.BoardId == boardId)
                .OrderBy(l => l.OrderIndex)
                .ToListAsync();
        }


        public async Task<List?> GetByIdAsync(int Id)
        {
                return await _Context.Lists
                .FirstOrDefaultAsync(l => l.Id == Id);
        }

        public async Task<int> Update(List list, int Id)
        {
return await _Context.Lists
                .Where(l => l.Id == Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(l => l.Title, list.Title)
                    .SetProperty(l => l.OrderIndex, list.OrderIndex)
                    .SetProperty(l => l.BoardId, list.BoardId)
                );
        }
        public async Task<int> GetMaxOrderIndexAsync(int boardId)
        {
            var max = await _Context.Lists
                .Where(l => l.BoardId == boardId)
                .Select(l => (int?)l.OrderIndex)
                .MaxAsync();

            return max ?? -1;

        }
    }
}
