using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class UserViewModel
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string GoogleId { get; set; }

        public DateTime Registered { get; set; }
    }
}
