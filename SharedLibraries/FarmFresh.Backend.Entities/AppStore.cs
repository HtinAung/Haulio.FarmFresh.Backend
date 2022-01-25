using System;
using System.Collections.Generic;

namespace FarmFresh.Backend.Entities
{
    public class AppStore : AppBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public virtual List<AppProduct> Products { get; set; } = new List<AppProduct>();

        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual List<AppOrderHistory> OrderHistories { get; set; } = new List<AppOrderHistory>();

    }
}
