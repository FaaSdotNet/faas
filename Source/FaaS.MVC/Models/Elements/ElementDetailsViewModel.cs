using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class ElementDetailsViewModel : ElementViewModel
    {
        [Display(Name = "Form")]
        public Guid FormId { get; set; }
    }
}
