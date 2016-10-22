using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FaaS.MVC.Models.Forms
{
    public class CreateFormViewModel : IValidatableObject
    {
        [Display(Name = "Project name")]
        public string Project;
        public SelectList ProjectList { get; set; }

        [Display(Name = "Form name")]
        public string FormName;

        public DateTime Created;

        public string Description;

        public IEnumerable Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}