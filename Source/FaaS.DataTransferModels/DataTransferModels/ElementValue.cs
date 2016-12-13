using System;

namespace FaaS.DataTransferModels
{
    public class ElementValue
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public Element Element { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Value)}: {Value}, {nameof(Element)}: {Element}";
        }
    }
}
