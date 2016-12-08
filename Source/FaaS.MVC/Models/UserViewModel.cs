using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class UserViewModel
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "User name")]
        public string Name { get; set; }

        public string GoogleToken { get; set; }

        public string Email { get; set; }

        public DateTime Registered { get; set; }

        public string AvatarUrl { get; set; }
    }
}
