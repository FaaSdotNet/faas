using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class ModelBase
    {
        /// <summary>
        /// <see cref="Id"/> is marked as (primary) key and while being a <see cref="Guid"/>, also DB generated. For <c>int</c> keys, autoincrement should be used by default.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Specifying unique constraint is not enough as string without fixed length (with max length) are hard to index and store.
        /// </summary>
        [Required, Index(IsUnique = true), MaxLength(254)]
        public string CodeName { get; set; }
    }
}
