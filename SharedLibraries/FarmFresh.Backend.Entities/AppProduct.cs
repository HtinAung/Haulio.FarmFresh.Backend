using System;

namespace FarmFresh.Backend.Entities
{
    public class AppProduct : AppBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int AvailableAmount { get; set; }
        public Guid CategoryId { get; set; }
        public Guid StoreId { get; set; }

        public virtual AppProductCategory Category { get; set; }
        public virtual AppStore Store { get; set; }
    }
}
