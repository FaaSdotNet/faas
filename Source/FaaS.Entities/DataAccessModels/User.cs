using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FaaS.Entities.DataAccessModels
{
    public class User : ModelBase
    {
        [Required]
        public string GoogleId { get; set; }

        [Required]
        public DateTime Registered { get; set; }

        /// <summary>
        /// Collection of all projects for user. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
