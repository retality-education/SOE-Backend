using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Book
    {
        public Guid BookId { get; set; }

        public string BookName { get; set; } = null!;

        public string Description { get; set; } = string.Empty;

        public string? Annotation { get; set; }

        public string Author { get; set; } = string.Empty;

        public string? CoverId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string Language { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }
        public bool IsAdultContent { get; set; } = false;
        public decimal? Rating { get; set; } 
    }
}
