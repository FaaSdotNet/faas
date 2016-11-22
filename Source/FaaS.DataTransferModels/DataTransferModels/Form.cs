using System;

namespace FaaS.DataTransferModels
{
    public class Form
    {
        public Guid Id { get; set; }

        public string FormName { get; set; }

        public DateTime Created { get; set; }

        public string Description { get; set; }

        public Project Project { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(FormName)}: {FormName}, {nameof(Created)}: {Created}, {nameof(Description)}: {Description}, {nameof(Project)}: {Project}";
        }
    }
}
