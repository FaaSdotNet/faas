using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class ElementValueViewModel
    {
        [Required]
        [Display(Name = "Code name")]
        public string ElementValueCodeName { get; set; }

        [Required]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        public string Value { get; set; }

        [Display(Name = "Element")]
        public string ElementName { get; set; }

        [Display(Name = "Session")]
        public string SessionName { get; set; }
    }
}
