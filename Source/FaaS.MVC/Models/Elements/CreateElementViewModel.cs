using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace FaaS.MVC.Models
{
    public enum InputType
    {
        CheckBox,
        Date,
        Radio,
        Range,
        Text
    }

    public class CreateElementViewModel : IValidatableObject
    {
        [Display(Name = "ID")]
        public Guid Id;

        public string Description { get; set; }

        public string Options { get; set; }

        public InputType Type { get; set; }

        public bool Required { get; set; }

        public SelectList ElementValueList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id.ToString() != UrlEncoder.Default.Encode(Id.ToString()))
            {
                yield return new ValidationResult("Invalid ID.");
            }
        }
    }
}
