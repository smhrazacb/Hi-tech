﻿namespace Ordering.Domain.Common
{
    public abstract class EntityBase
    {
        public int OrderId { get; protected set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
