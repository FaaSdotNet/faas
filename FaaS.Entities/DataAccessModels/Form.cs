using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class Form : ModelBase
    {
        [ForeignKey("Project")]
        public Guid ProjectId { get; set; }

        [Required]
        public Project Project { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Collection of all elements for a form. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<Element> Elements { get; set; } = new List<Element>();
    }
}
