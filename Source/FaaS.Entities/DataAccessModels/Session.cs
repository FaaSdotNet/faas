using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class Session : ModelBase
    {
        [Required]
        public DateTime Filled { get; set; }

        /// <summary>
        /// Collection of all values for a session. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<ElementValue> ElementValues { get; set; } = new List<ElementValue>();
    }
}
