using System;

namespace FaaS.Services.DataTransferModels
{
    public class Form
    {
        public string FormCodeName { get; set; }

        public string DisplayName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public Project Project { get; set; }
    }
}
