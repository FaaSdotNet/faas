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
        public Guid Id { get; set; }

        [Required]
        [Display(Name ="Form name")]
        public string FormName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public SelectList ElementList { get; set; }
        
        public SelectList ProjectList { get; set; }

        public Guid SelectedProjectId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id.ToString() != UrlEncoder.Default.Encode(Id.ToString()))
            {
                yield return new ValidationResult("Invalid code name");
            }
        }
    }
}