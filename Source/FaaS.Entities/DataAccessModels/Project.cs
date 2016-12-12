using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class Project : ModelBase
    {
        [Required, MaxLength(254)]
        public string Name { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        
        [Required]
        public virtual User User { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Collection of all forms for a project. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<Form> Forms { get; set; } = new List<Form>();
    }
}
