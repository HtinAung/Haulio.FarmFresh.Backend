using System;

namespace FarmFresh.Backend.Api.Customers.Models
{
    public class CreateOrderRequest
    {
        public int Total { get; set; }
        public Nullable<Guid> ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid StoreId { get; set; }
    }
}
