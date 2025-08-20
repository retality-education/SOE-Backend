using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Application.Interfaces.Auth;
namespace Infrastructure.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password) => 
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);    
        public bool Verify(string password, string hashPassword) => 
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
    }
}
