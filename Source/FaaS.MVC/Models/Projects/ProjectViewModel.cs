using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class ProjectViewModel
    {
        [Display(Name = "Code name")]
        public string ProjectCodeName { get; set; }

        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }
    }
}
