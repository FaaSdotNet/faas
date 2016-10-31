using Microsoft.AspNetCore.Mvc.Rendering;
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
        public CreateElementViewModel()
        {
            ElementCodeName = "";
        }

        //[Required]
        [Display(Name = "Code name")]
        public string ElementCodeName;

        //[Required]
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Options { get; set; }

        public InputType Type { get; set; }

        public bool Mandatory { get; set; }

        public SelectList ElementValueList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ElementCodeName != UrlEncoder.Default.Encode(ElementCodeName))
            {
                yield return new ValidationResult("Invalid code name");
            }
        }
    }
}
