using System;

namespace FarmFresh.Backend.Entities
{
    public class AppUserAddress
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }

        public string PostalCode { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
