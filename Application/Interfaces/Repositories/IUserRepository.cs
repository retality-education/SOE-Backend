using Core.Models;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task AddAsync(User user);
        public Task<User?> GetByEmailAsync(string email);
        public Task<bool> ExistsByEmailAsync(string email);
        public Task<bool> ExistsByIdAsync(Guid userId);
        public Task<User?> GetByIdAsync(Guid userId);
        public Task ChangePasswordAsync(User user, string newPassword);
        public Task UpdateAsync(User user);

    }
}