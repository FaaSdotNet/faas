using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class ProjectViewModel
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Project name")]
        public string ProjectName { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }
    }
}
