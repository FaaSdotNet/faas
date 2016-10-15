using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class ElementValue : ModelBase
    {
        [ForeignKey("Element")]
        public Guid ElementId { get; set; }

        [Required]
        public Element Element { get; set; }

        [ForeignKey("Session")]
        public Guid SessionId { get; set; }

        [Required]
        public Session Session { get; set; }

        public string Value { get; set; }
    }
}
