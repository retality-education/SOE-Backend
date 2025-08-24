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
    public class CatalogDataRepository : ICatalogDataRepository
    {
        private readonly SoeBackendContext _context;
        private readonly IMapper _mapper;

        public CatalogDataRepository(
            SoeBackendContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            var tagEntities = await _context.Tags
                .OrderBy(t => t.TagName)
                .ToListAsync()
                .ConfigureAwait(false);

            return _mapper.Map<List<Tag>>(tagEntities);
        }

        public async Task<List<Genre>> GetGenresAsync()
        {
            var genreEntities = await _context.Genres
                .OrderBy(g => g.GenreName)
                .ToListAsync()
                .ConfigureAwait(false);

            return _mapper.Map<List<Genre>>(genreEntities);
        }

        public async Task<List<SortOption>> GetSortOptionsAsync()
        {
            var sortOptionEntities = await _context.SortOptions
                .Where(so => so.IsActive)
                .OrderBy(so => so.Order)
                .ToListAsync()
                .ConfigureAwait(false);

            return _mapper.Map<List<SortOption>>(sortOptionEntities);
        }


        public async Task AddTagAsync(Tag tag)
        {
            var tagEntity = _mapper.Map<TagEntity>(tag);
            await _context.Tags.AddAsync(tagEntity).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task AddGenreAsync(Genre genre)
        {
            var genreEntity = _mapper.Map<GenreEntity>(genre);
            await _context.Genres.AddAsync(genreEntity).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task AddSortOptionAsync(SortOption sortOption)
        {
            var sortOptionEntity = _mapper.Map<SortOptionEntity>(sortOption);
            await _context.SortOptions.AddAsync(sortOptionEntity).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

