using System;

namespace FarmFresh.Backend.Web.Customer.Models
{
    public class CreateOrderViewModel
    {
        public int Total { get; set; }
        public Nullable<Guid> ProductId { get; set; }
        public Guid StoreId { get; set; }
    }
}
