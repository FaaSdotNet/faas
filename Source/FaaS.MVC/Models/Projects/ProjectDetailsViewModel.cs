using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models.Projects
{
    public class ProjectDetailsViewModel : ProjectViewModel
    {
        [Display(Name = "User")]
        public string UserName { get; set; }
    }
}
