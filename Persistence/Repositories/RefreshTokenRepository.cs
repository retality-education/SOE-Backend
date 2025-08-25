using Application.Interfaces.Repositories;
using AutoMapper;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly SoeBackendContext _context;
        private readonly IMapper _mapper;
        public RefreshTokenRepository(SoeBackendContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            var rtoken = await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.Token == token).ConfigureAwait(false);

            if (rtoken is null)
                throw new Exception("Refresh token is invalid!");
            return _mapper.Map<RefreshToken>(rtoken);
        }

        public async Task ReplaceAsync(Guid oldTokenId, RefreshToken newToken)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync().ConfigureAwait(false);

            try
            {
                // Отзываем старый токен
                RefreshTokenEntity? oldToken = await _context.RefreshTokens.FindAsync(oldTokenId).ConfigureAwait(false);
                if (oldToken is not null)
                    oldToken.IsRevoked = true;

                var rtoken = _mapper.Map<RefreshTokenEntity>(newToken);

                _context.RefreshTokens.Add(rtoken);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                await transaction.CommitAsync().ConfigureAwait(false);
            }
            catch
            {
                await transaction.RollbackAsync().ConfigureAwait(false);
                throw;
            }
        }

        public async Task RevokeAsync(Guid tokenId)
        {
            var rtoken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == tokenId).ConfigureAwait(false);

            if (rtoken is null)
                throw new Exception("Refresh token is invalid!");
            
            rtoken.IsRevoked = true;
            await _context.SaveChangesAsync().ConfigureAwait(false);

        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            var rtoken = _mapper.Map<RefreshTokenEntity>(refreshToken);

            await _context.RefreshTokens.AddAsync(rtoken).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
