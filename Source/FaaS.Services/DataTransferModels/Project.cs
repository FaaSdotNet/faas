using System;

namespace FaaS.Services.DataTransferModels
{
    public class Project
    {
        public Guid Id { get; set; }

        public string ProjectName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public User User { get; set; }
    }
}
