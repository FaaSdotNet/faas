using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class Form
    {
        /// <summary>
        /// <see cref="Id"/> is marked as (primary) key and while being a <see cref="Guid"/>, also DB generated. For <c>int</c> keys, autoincrement should be used by default.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, Index("IX_FormUnique", 1, IsUnique = true), MaxLength(254)]
        public string Name { get; set; }

        [ForeignKey("Project"), Index("IX_FormUnique", 2, IsUnique = true)]
        public Guid ProjectId { get; set; }

        [Required]
        public Project Project { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Collection of all elements for a form. Effectively creating 1:N relation.
        /// </summary>
        public virtual ICollection<Element> Elements { get; set; } = new List<Element>();
    }
}
