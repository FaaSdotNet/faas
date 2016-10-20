using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class Element : ModelBase
    {
        [ForeignKey("Form")]
        public Guid FormId { get; set; }

        [Required]
        public Form Form { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Options { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public bool Mandatory { get; set; }

        /// <summary>
        /// Collection of all values for an element. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<ElementValue> ElementValues { get; set; } = new List<ElementValue>();
    }
}
