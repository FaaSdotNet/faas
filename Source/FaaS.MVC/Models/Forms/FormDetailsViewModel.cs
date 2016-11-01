using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class FormDetailsViewModel : FormViewModel
    {
        [Display(Name = "Project")]
        public Guid ProjectId { get; set; }
    }
}