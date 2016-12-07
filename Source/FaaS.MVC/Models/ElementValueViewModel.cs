using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class ElementValueViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        public string Value { get; set; }

        [Display(Name = "Element")]
        public Guid ElementId { get; set; }

        [Display(Name = "Session")]
        public Guid SessionId { get; set; }
    }
}
