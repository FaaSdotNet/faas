using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class UserViewModel
    {
        [Display(Name = "Code name")]
        public string UserCodeName { get; set; }

        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        public string GoogleId { get; set; }

        public DateTime Registered { get; set; }
    }
}
