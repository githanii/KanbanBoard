using Application.Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IUintofWork

    {
            IBoardRepository Boards { get; }
            IListRepository Lists { get; }
            ICardRepository Cards { get; }
            IUserRepository Usres { get; }
            IRefreshToken RefreshTokens { get; }
            Task<int> SaveChangesAsync();
        }
    }

