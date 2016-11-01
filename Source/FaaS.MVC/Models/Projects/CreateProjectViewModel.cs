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
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Required]
        public string ProjectName { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public CreateProjectViewModel()
        {
            Created = DateTime.Now;
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
