using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaaS.Entities.DataAccessModels
{
    public class ElementValue : ModelBase
    {
        [ForeignKey("Element")]
        public Guid ElementId { get; set; }
        
        public virtual Element Element { get; set; }

        [ForeignKey("Session")]
        public Guid SessionId { get; set; }
        
        public virtual Session Session { get; set; }
        
        public string Value { get; set; }
    }
}
