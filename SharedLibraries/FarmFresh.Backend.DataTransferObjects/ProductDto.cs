using System;
using System.IO;

namespace FarmFresh.Backend.DataTransferObjects
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Stream UploadedImage { get; set; }
        public string UploadedImageExtension { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int AvailableAmount { get; set; }
        public Guid StoreId { get; set; }
        public Guid CategoryId { get; set; }
        public string StoreName { get; set; }
        public string CategoryName { get; set; }
    }
}
