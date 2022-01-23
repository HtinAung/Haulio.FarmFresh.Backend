using System;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.Backend.Api.Stores.Models
{
    public class DeleteProductRequest
    {
        [Required]
        public Guid ProductId { get; set; }
    }
}
