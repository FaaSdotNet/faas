using System;

namespace FaaS.Services.DataTransferModels
{
    public class Project
    {

        public string ProjectCodeName { get; set; }

        public string DisplayName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public User User { get; set; }
    }
}
