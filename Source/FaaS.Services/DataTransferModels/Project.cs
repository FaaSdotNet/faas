using System;

namespace FaaS.Services.DataTransferModels
{
    public class Project
    {
        public Guid Id { get; set; }

        public string Name{ get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public User User { get; set; }
    }
}
