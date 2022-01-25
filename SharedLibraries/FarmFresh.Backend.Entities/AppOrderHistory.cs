using System;

namespace FarmFresh.Backend.Entities
{
    public class AppOrderHistory : AppBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Item { get; set; }
        public int Total { get; set; }
        public decimal Price { get; set; }
        public Guid UserId { get; set; }
        public Guid StoreId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual AppStore Store { get; set; }
    }
}
