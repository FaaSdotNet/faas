using System;

namespace FaaS.DataTransferModels
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string GoogleToken { get; set; }

        public string Email { get; set; }

        public DateTime Registered { get; set; }

        public string AvatarUrl { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(GoogleToken)}: {GoogleToken}, {nameof(Email)}: {Email}, {nameof(Registered)}: {Registered}, {nameof(AvatarUrl)}: {AvatarUrl}";
        }
    }
}
