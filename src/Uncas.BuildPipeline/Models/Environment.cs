namespace Uncas.BuildPipeline.Models
{
    using System.Collections.Generic;

    public class Environment
    {
        private readonly IDictionary<string, string> _properties;

        public Environment()
        {
            _properties = new Dictionary<string, string>();
        }

        public int Id { get; set; }
        public string EnvironmentName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Properties
        {
            get { return _properties; }
        }

        public void AddProperty(string key, string value)
        {
            _properties.Add(key, value);
        }
    }
}