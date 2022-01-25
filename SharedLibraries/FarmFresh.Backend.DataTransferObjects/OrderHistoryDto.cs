using System;
namespace FarmFresh.Backend.DataTransferObjects
{
    public class OrderHistoryDto
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
