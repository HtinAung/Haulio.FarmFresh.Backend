using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmFresh.Backend.Web.Customer.Models
{
    public class OrderHistoryViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Item { get; set; }
        public int Total { get; set; }
        public decimal Price { get; set; }
        public Nullable<Guid> ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid StoreId { get; set; }
        public string UserName { get; set; }
        public string StoreName { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
