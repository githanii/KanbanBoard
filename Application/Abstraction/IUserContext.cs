using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
        public interface IUserContext
        {
            int GetCurrentUserId();
            string? GetCurrentUserRole();
            string? GetCurrentUserName();
            bool IsAuthenticated();
            bool IsInRole(string role);


   

        }
    }


