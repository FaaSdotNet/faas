using System;

namespace FaaS.Services.DataTransferModels
{
    public class User
    {
        public string UserCodeName { get; set; }

        public string DisplayName { get; set; }

        public string GoogleId { get; set; }

        public DateTime Registered { get; set; }
    }
}
