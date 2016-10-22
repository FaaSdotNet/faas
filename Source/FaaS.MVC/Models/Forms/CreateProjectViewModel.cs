using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FaaS.MVC.Models.Forms
{
    public class CreateProjectViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "Code name")]
        public string CodeName { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CodeName != UrlEncoder.Default.Encode(CodeName))
            {
                yield return new ValidationResult("Invalid code name");
            }
        }
    }
}
