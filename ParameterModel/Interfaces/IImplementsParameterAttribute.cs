using System.Text.Json.Serialization;

namespace ParameterModel.Interfaces
{
    /// <summary>
    /// Marker interface.
    /// Add this to any class with properties that will be attributed with ParameterPromptAttribute.
    /// </summary>
    public interface IImplementsParameterAttribute
    {
        /// <summary>
        /// Maps the variable assignments to the property names.
        /// </summary>
        Dictionary<string, string> VariableAssignments { get; }
        /// <summary>
        /// Holds a map of models for each property with a ParameterAttribute.
        /// </summary>
        [JsonIgnore]
        Dictionary<string, IParameterModel> ParameterMap { get; }
    }
}
