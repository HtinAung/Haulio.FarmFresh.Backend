using System;

namespace FarmFresh.Backend.DataTransferObjects
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string StoreName { get; set; }
        public Nullable<Guid> StoreId { get; set; }
    }
}
