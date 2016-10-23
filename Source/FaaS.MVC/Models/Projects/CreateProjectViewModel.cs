using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace FaaS.MVC.Models
{
    public class CreateProjectViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "Code name")]
        public string ProjectCodeName { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public SelectList FormList { get; set; }

        public string UserName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectCodeName != UrlEncoder.Default.Encode(ProjectCodeName))
            {
                yield return new ValidationResult("Invalid code name");
            }
        }
    }
}
