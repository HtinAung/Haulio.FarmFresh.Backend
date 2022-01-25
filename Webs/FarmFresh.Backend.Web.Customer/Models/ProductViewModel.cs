using System;

namespace FarmFresh.Backend.Web.Customer.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int AvailableAmount { get; set; }
        public Guid StoreId { get; set; }
        public Guid CategoryId { get; set; }
        public string StoreName { get; set; }
        public string CategoryName { get; set; }
    }
}
