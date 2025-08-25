using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    // Сервис как UseCase, с одним методом для проверки существования юзера по id
    public class ValidationService
    {
        private readonly IUserRepository _userRepository;
        public ValidationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> IsUserExistById(Guid userId)
        {
            return await _userRepository.ExistsByIdAsync(userId).ConfigureAwait(false);
        }
    }
}
