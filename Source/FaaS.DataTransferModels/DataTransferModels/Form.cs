using System;

namespace FaaS.DataTransferModels
{
    public class Form
    {
        public Guid Id { get; set; }

        public string FormName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public Project Project { get; set; }
    }
}
