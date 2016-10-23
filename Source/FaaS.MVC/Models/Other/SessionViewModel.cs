using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class SessionViewModel
    {
        [Required]
        [Display(Name = "Code name")]
        public string SessionCodeName { get; set; }

        [Required]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        [Required]
        public DateTime Filled { get; set; }

        public SelectList ElementValueList { get; set; }
    }
}
