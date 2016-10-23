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
        [Display(Name = "Code name")]
        public string UserCodeName { get; set; }

        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        [Required]
        public string GoogleId { get; set; }

        public DateTime Registered { get; set; }

        public SelectList ProjectList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserCodeName != UrlEncoder.Default.Encode(UserCodeName))
            {
                yield return new ValidationResult("Invalid code name");
            }
        }
    }
}
