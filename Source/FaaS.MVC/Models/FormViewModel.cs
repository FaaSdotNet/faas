using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class FormViewModel
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Form name")]
        public string FormName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public Guid ProjectId { get; set; }
    }
}
