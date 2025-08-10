using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Data.DTOs
{
    public class CompanySignUpDto
    {
        [Required(ErrorMessage = "Arabic Name is required")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "English Name is required")]
        public string NameEn { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        [Url(ErrorMessage = "Invalid Website URL")]
        public string? WebsiteUrl { get; set; }

        public IFormFile? Logo { get; set; }
    }
}
