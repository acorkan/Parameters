using ParameterModel.Attributes;
using System.Reflection;

namespace ParameterModel.Interfaces
{
    /// <summary>
    /// Marker interface.
    /// Add this to any class with properties that will be attributed with ParameterPromptAttribute.
    /// </summary>
    public interface IImplementsParameterAttribute
    {
        //Dictionary<PropertyInfo, ParameterAttribute> GetAttributeMap();
    }
}
