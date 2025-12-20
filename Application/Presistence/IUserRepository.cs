using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Presistence
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByUserIdAsync(int useId);
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);

        Task<int> UpdateAsync(User user);
        Task<int> DeleteAsync(int userId);  


    }
}
