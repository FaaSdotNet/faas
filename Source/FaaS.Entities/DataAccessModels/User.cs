using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class User : ModelBase
    {
        [Required, MaxLength(254)]
        public string Name { get; set; }

        [Required]
        public string GoogleToken { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime Registered { get; set; }

        [Required]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Collection of all projects for user. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
