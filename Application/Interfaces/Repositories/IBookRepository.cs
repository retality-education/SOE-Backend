using Application.DTOs;
using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<PagedResult<Book>> GetBooksPagedAsync(BookFilter bookFilter, int page = 1, int pageSize = 32);
    }
}
