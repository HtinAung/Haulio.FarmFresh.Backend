using System;
using System.Collections.Generic;

namespace FarmFresh.Backend.Entities
{
    public class AppProductCategory: AppBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public virtual List<AppProduct> Products { get; set; } = new List<AppProduct>();
    }
}
