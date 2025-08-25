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
    public class UserRepository : IUserRepository
    {
        private readonly SoeBackendContext _context;
        private readonly IMapper _mapper;
        public UserRepository(SoeBackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddAsync(User user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            await _context.Users.AddAsync(userEntity).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or whitespace.", nameof(email));
            }

            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email).ConfigureAwait(false);

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
                .AnyAsync(u => u.Email == email).ConfigureAwait(false);
        }
        public async Task<bool> ExistsByIdAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.UserId == userId).ConfigureAwait(false);
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId).ConfigureAwait(false);

            if (userEntity is null)
            {
                return null;
            }

            return _mapper.Map<User>(userEntity);
        }

        public async Task ChangePasswordAsync(User user, string newPassword)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.Id).ConfigureAwait(false);

            if (userEntity is null)
                throw new ArgumentException("User not exist");

            userEntity.PasswordHash = newPassword;

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var entity = await _context.Users.FindAsync(user.Id).ConfigureAwait(false);
            if (entity is null)
                throw new ArgumentException("User with this id not exist");

            _mapper.Map(user, entity);

            _context.Users.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
