using System;

namespace FaaS.Services.DataTransferModels
{
    public class Session
    {
        public string SessionCodeName { get; set; }

        public string DisplayName { get; set; }

        public DateTime Filled { get; set; }
    }
}
