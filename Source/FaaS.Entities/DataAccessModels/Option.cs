using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class Option : ModelBase
    {
        [ForeignKey("Element")]
        public Guid ElementId { get; set; }
        
        [Required]
        public Element Element { get; set; }

        [Required]
        public string Label { get; set; }
    }
}
