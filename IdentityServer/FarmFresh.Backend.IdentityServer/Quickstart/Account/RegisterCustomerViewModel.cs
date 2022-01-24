using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Quickstart.UI
{
    public class RegisterCustomerViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        public string Password { get; set; }
        [Required]
        [MinLength(2)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Address Line")]
        public string AddressLine { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        public string ReturnUrl { get; set; }
    }
}
