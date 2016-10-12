using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class Project : ModelBase
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        [Required]
        public User User { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Collection of all forms for a project. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<Form> Forms { get; set; } = new List<Form>();
    }
}
