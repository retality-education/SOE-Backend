using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class RefreshTokenEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual UserEntity User { get; set; } = null!;
}
