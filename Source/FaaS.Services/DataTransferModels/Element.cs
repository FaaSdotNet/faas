using System;

namespace FaaS.Services.DataTransferModels
{
    public class Element
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Options { get; set; }

        public int Type { get; set; }

        public bool Mandatory { get; set; }

        public Form Form { get; set; }
    }
}
