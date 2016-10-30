using System;

namespace FaaS.Services.DataTransferModels
{
    public class Session
    {
        public Guid Id { get; set; }

        public DateTime Filled { get; set; }
    }
}
