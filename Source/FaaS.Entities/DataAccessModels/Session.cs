using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class Session
    {
        /// <summary>
        /// <see cref="Id"/> is marked as (primary) key and while being a <see cref="Guid"/>, also DB generated. For <c>int</c> keys, autoincrement should be used by default.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public DateTime Filled { get; set; }

        /// <summary>
        /// Collection of all values for a session. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<ElementValue> ElementValues { get; set; } = new List<ElementValue>();
    }
}
