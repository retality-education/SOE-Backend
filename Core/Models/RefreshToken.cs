using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }  // Связь с пользователем
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }  // Срок действия
        public bool IsRevoked { get; set; } = false;  // Отозван ли токен
    }
}
