using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace FaaS.MVC.Models
{
    public class CreateFormViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "Code name")]
        public string FormCodeName;

        [Required]
        public string DisplayName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public SelectList ElementList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FormCodeName != UrlEncoder.Default.Encode(FormCodeName))
            {
                yield return new ValidationResult("Invalid code name");
            }
        }
    }
}