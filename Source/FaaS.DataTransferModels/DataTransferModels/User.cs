using System;

namespace FaaS.DataTransferModels
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string GoogleId { get; set; }

        public DateTime Registered { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(UserName)}: {UserName}, {nameof(GoogleId)}: {GoogleId}, {nameof(Registered)}: {Registered}";
        }
    }
}
