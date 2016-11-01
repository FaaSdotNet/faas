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
    }
}
