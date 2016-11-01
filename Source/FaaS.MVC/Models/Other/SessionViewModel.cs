using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class SessionViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Required]
        public DateTime Filled { get; set; }

        public SelectList ElementValueList { get; set; }
    }
}
