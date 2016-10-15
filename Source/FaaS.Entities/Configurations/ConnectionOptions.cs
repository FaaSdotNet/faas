namespace FaaS.Entities.Configuration
{
    /// <summary>
    /// Class is used to deliver application-level configuration to EF contexts.
    /// </summary>
    public class ConnectionOptions
    {
        public string ConnectionString { get; set; }
    }
}
