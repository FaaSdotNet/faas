namespace FaaS.Services.DataTransferModels
{
    public class Element
    {
        public string ElementCodeName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Options { get; set; }

        public int Type { get; set; }

        public bool Mandatory { get; set; }

        public Form Form { get; set; }
    }
}
