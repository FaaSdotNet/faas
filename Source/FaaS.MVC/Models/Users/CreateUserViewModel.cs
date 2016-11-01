using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace FaaS.MVC.Models
{
    public class CreateUserViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        public string GoogleId { get; set; }

        [Required]
        public DateTime Registered { get; set; }

        public CreateUserViewModel()
        {
            Registered = DateTime.Now;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id.ToString() != UrlEncoder.Default.Encode(Id.ToString()))
            {
                yield return new ValidationResult("Invalid code name");
            }
        }
    }
}
