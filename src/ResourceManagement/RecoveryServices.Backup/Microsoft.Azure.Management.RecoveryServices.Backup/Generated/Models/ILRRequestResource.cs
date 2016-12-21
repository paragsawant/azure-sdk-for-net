// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.RecoveryServices.Backup.Models
{
    using System.Linq;

    /// <summary>
    /// Parameters to restore file/folders API.
    /// </summary>
    public partial class ILRRequestResource : Resource
    {
        /// <summary>
        /// Initializes a new instance of the ILRRequestResource class.
        /// </summary>
        public ILRRequestResource() { }

        /// <summary>
        /// Initializes a new instance of the ILRRequestResource class.
        /// </summary>
        /// <param name="id">Resource Id represents the complete path to the
        /// resource.</param>
        /// <param name="name">Resource name associated with the
        /// resource.</param>
        /// <param name="type">Resource type represents the complete path of
        /// the form Namespace/ResourceType/ResourceType/...</param>
        /// <param name="location">Resource location.</param>
        /// <param name="tags">Resource tags.</param>
        /// <param name="eTag">Optional ETag.</param>
        public ILRRequestResource(string id = default(string), string name = default(string), string type = default(string), string location = default(string), System.Collections.Generic.IDictionary<string, string> tags = default(System.Collections.Generic.IDictionary<string, string>), string eTag = default(string), ILRRequest properties = default(ILRRequest))
            : base(id, name, type, location, tags, eTag)
        {
            Properties = properties;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "properties")]
        public ILRRequest Properties { get; set; }

    }
}