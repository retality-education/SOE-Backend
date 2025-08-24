using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ICatalogDataRepository
    {
        Task<List<Tag>> GetTagsAsync();
        Task<List<Genre>> GetGenresAsync();
        Task<List<SortOption>> GetSortOptionsAsync();

        // Для админки
        Task AddTagAsync(Tag tag);
        Task AddGenreAsync(Genre genre);
        Task AddSortOptionAsync(SortOption sortOption);
    }
}
