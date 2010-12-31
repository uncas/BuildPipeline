namespace Uncas.BuildPipeline.Models
{
    using System.Collections.Generic;

    public class Environment
    {
        private readonly IDictionary<string, string> properties;

        public Environment()
        {
            this.properties = new Dictionary<string, string>();
        }

        public int Id { get; set; }
        public string EnvironmentName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Properties
        {
            get
            {
                return this.properties;
            }
        }

        public void AddProperty(string key, string value)
        {
            this.properties.Add(key, value);
        }
    }
}