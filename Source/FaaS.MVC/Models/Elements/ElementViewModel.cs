using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.MVC.Models
{
    public class ElementViewModel
    {
        [Display(Name = "Code name")]
        public string ElementCodeName { get; set; }

        [Display(Name = "Display name")]
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Options { get; set; }

        public int Type { get; set; }

        public bool Mandatory { get; set; }
    }
}
