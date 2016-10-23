using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class ProjectDetailsViewModel : ProjectViewModel
    {
        [Display(Name = "User")]
        public string UserName { get; set; }
    }
}
