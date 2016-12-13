using System;
using System.Collections.Generic;

namespace FaaS.DataTransferModels
{
    public class Session
    {
        public Guid Id { get; set; }

        public DateTime Filled { get; set; }

        public virtual ICollection<ElementValue> ElementValues { get; set; } = new List<ElementValue>();

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Filled)}: {Filled}";
        }
    }

}
