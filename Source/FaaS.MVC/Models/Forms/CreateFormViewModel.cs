using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace FaaS.MVC.Models
{
    public class CreateFormViewModel
    {
        /*[Required]    // user (probably) doesn't set codename
        [Display(Name = "Code name")]*/ 
        public string FormCodeName { get; set; }

        [Required]
        [Display(Name ="Form name")]
        public string DisplayName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public SelectList ElementList { get; set; }
        
        public SelectList ProjectList { get; set; }

        public string SelectedProjectCodeName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FormCodeName != UrlEncoder.Default.Encode(DisplayName))
            {
                yield return new ValidationResult("Invalid code name");
            }
        }
    }
}