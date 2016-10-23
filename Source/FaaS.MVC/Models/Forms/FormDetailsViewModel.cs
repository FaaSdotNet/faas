using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public class FormDetailsViewModel : FormViewModel
    {
        [Display(Name = "Project")]
        public string ProjectName { get; set; }
    }
}