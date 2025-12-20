using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Application.Abstraction
{
    public interface ITokenService
    {

        string GenerateAccessToken(int userId, string role, string username);

        string GenerateRefreshToken();

        ClaimsPrincipal? ValidateAccessToken(string token);

        string GetSha256Hash(string input);


    }


    
}
