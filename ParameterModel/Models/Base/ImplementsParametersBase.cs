using ParameterModel.Attributes;
using ParameterModel.Interfaces;

namespace ParameterModel.Models.Base
{
    public class ImplementsParametersBase : IImplementsParameterAttribute
    {
        public Dictionary<string, string> VariableAssignments { get; } = new Dictionary<string, string>();
        public Dictionary<string, ParameterAttribute> AttributeMap { get; } = new Dictionary<string, ParameterAttribute>();
    }
}
