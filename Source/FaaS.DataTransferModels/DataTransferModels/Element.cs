using System;

namespace FaaS.DataTransferModels
{
    public class Element
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Options { get; set; }

        public int Type { get; set; }

        public bool Required { get; set; }

        public Form Form { get; set; }
    }
}
