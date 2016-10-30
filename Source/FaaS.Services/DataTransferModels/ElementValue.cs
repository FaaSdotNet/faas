using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaaS.Services.DataTransferModels
{
    public class ElementValue
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public Element Element { get; set; }

        public Session Session { get; set; }
    }
}
