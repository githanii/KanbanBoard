using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Presistence
{
    public interface IListRepository
    {
        Task<List?>  GetByIdAsync(int Id);
        Task<IEnumerable<List>> GetAllByBoardAsync(int boardId);
        Task<int> AddAsync(List list);
        Task<int> DeleteAsync(int Id);
        Task<int> Update(List list, int Id);
        Task<int> GetMaxOrderIndexAsync(int boardId);

    }
}
