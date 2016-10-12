using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaaS.Entities.DataAccessModels
{
    public class Element : ModelBase
    {
        [ForeignKey("Form")]
        public int FormId { get; set; }

        [Required]
        public Form Form { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public bool Mandatory { get; set; }

        /// <summary>
        /// Collection of all options for an element. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<Option> Options { get; set; } = new List<Option>();

        /// <summary>
        /// Collection of all values for an element. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<ElementValue> ElementValues { get; set; } = new List<ElementValue>();
    }
}
