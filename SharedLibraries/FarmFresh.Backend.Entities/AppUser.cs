using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FarmFresh.Backend.Entities
{
    public class AppUser: IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public Nullable<Guid> StoreId { get; set; }
        public virtual AppStore Store { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public virtual List<AppUserAddress> Addresses { get; set; } = new List<AppUserAddress>();
        public virtual List<AppOrderHistory> OrderHistories { get; set; } = new List<AppOrderHistory>();
    }

}
