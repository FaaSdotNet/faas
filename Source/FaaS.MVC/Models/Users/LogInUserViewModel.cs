using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace FaaS.MVC.Models
{
    public class LogInUserViewModel : IValidatableObject
    {
        [Required]
        public string GoogleId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (GoogleId != UrlEncoder.Default.Encode(GoogleId))
            {
                yield return new ValidationResult("Invalid code name");
            }
        }
    }
}
