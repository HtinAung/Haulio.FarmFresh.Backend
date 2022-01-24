using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.Backend.Api.Stores.Models
{
    public class UpdateProductRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public IFormFile Image { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int AvailableAmount { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}
