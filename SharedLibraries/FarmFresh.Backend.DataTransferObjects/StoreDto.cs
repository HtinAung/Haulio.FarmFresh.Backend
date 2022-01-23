using System;

namespace FarmFresh.Backend.DataTransferObjects
{
    public class StoreDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Guid AdminUserId { get; set; }
        public string AdminUserName { get; set; }
    }
}
