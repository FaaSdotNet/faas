using System;

namespace FaaS.DataTransferModels
{
    public class Project
    {
        public Guid Id { get; set; }

        public string ProjectName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public User User { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(ProjectName)}: {ProjectName}, {nameof(Created)}: {Created}, {nameof(Description)}: {Description}, {nameof(User)}: {User}";
        }
    }
}
