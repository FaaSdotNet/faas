using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FaaS.Services.DataTransferModels;

namespace FaaS.MVC.Models.Projects
{
    public class ProjectModelView
    {
        [Display(Name = "Code name")]
        public string ProjectCodeName { get; set; }

        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public String User { get; set; }
    }
}
