namespace Garcia.Infrastructure.ElasticSearch
{
    public class ElasticSearchSettings
    {
        public ElasticSearchSettings()
        {
            AuthenticationRequired = !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }
        public string Uri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool AuthenticationRequired { get; }
    }
}