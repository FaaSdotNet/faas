using System;

namespace FaaS.Services.DataTransferModels
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string GoogleId { get; set; }

        public DateTime Registered { get; set; }
    }
}
