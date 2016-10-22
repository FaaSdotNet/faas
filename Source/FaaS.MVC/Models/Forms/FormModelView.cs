using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.MVC.Models.Forms
{
    public class FormModelView
    {
        [Display(Name = "Project name")]
        public string ProjectName { get; set; }

        [Display(Name = "Form name")]
        public string FormName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }
    }
}
