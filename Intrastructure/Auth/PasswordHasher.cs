using Application.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Intrastructure.Auth
{
    public class PasswordHasher: IPasswordHasher
    {
        private readonly PasswordHasher<object> _hasher = new();
        public string HashPassword(string password)
        
           => _hasher.HashPassword(null!, password);
        
        public bool VerifyPassword(string hashedPassword, string providedPassword)
              => _hasher.VerifyHashedPassword(null!, hashedPassword, providedPassword) 
            == PasswordVerificationResult.Success;

    }
}
