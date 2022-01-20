using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FarmFresh.Backend.Entities
{
    public class AppUser: IdentityUser<Guid>
    {
        public string FullName { get; set; }

        public virtual List<AppUserAddress> Address { get; set; } = new List<AppUserAddress>();
    }

}
