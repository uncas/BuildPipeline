namespace Uncas.BuildPipeline.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Environment
    {
        private readonly IDictionary<string, EnvironmentProperty> _properties;

        public Environment()
        {
            _properties = new Dictionary<string, EnvironmentProperty>();
        }

        public int Id { get; set; }

        public string EnvironmentName { get; set; }

        public IEnumerable<EnvironmentProperty> Properties
        {
            get { return _properties.Select(x => x.Value); }
        }

        public void AddProperty(string key, string value)
        {
            _properties.Add(
                key,
                new EnvironmentProperty { Key = key, Value = value });
        }
    }
}