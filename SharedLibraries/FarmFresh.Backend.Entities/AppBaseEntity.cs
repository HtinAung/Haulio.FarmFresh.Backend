using System;

namespace FarmFresh.Backend.Entities
{
    public class AppBaseEntity
    {
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
