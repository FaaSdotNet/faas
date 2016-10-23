using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class FormViewModel
    {
        [Display(Name = "Code name")]
        public string FormCodeName { get; set; }

        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }
    }
}
