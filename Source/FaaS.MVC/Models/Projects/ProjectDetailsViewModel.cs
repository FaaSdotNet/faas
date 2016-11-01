using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class ProjectDetailsViewModel : ProjectViewModel
    {
        [Display(Name = "User")]
        public Guid UserId { get; set; }
    }
}
