using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.Backend.Api.Stores.Models
{
    public class CreateNewProductRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int AvailableAmount { get; set; }
        
        [Required]
        public Guid CategoryId { get; set; }
    }
   
}
