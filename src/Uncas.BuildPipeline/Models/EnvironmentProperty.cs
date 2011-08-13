namespace Uncas.BuildPipeline.Models
{
    /// <summary>
    /// Represents a property of an environment.
    /// </summary>
    public class EnvironmentProperty
    {
        /// <summary>
        /// Gets or sets the key of the environment property.
        /// </summary>
        /// <value>
        /// The key of the environment property.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value of the environment property.
        /// </summary>
        /// <value>
        /// The value of the environment property.
        /// </value>
        public string Value { get; set; }
    }
}