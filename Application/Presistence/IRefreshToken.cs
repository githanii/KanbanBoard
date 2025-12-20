using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Presistence
{
    public interface IRefreshToken
    {
        Task<RefreshToken?> GetByTokenAsync(string tokenhHash);
        Task AddAsync(RefreshToken refreshToken);
        Task RemoveUserTokensAsync(int userId);


    }
}
