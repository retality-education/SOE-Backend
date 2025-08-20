using Application.Interfaces.Repositories;
using AutoMapper;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly SoeBackendContext _context;
        private readonly IMapper _mapper;
        public UsersRepository(SoeBackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddAsync(User user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or whitespace.", nameof(email));
            }

            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (userEntity is null)
            {
                return null;
            }

            return _mapper.Map<User>(userEntity);
        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (userEntity is null)
            {
                return null;
            }

            return _mapper.Map<User>(userEntity);
        }

        public async Task ChangePasswordAsync(User user, string newPassword)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.Id);

            if (userEntity is null)
                throw new ArgumentException("User not exist");

            userEntity.PasswordHash = newPassword;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {

            var entity = await _context.Users.FindAsync(user.Id);
            if (entity is null)
                throw new ArgumentException("User with this id not exist");

            _mapper.Map(user, entity);

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
