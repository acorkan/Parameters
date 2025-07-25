using ParameterModel.Factories;
using ParameterModel.Interfaces;
using System.Text.Json.Serialization;

namespace ParameterModel.Models.Base
{
    /// <summary>
    /// Base class to simplify implementation of IImplementsParameterAttribute.
    /// It will automatically build the AttributeMap from the properties that have a ParameterAttribute, 
    /// and initialize a blank VariableAssignments.
    /// 
    /// </summary>
    public abstract class ImplementsParametersBase : IImplementsParameterAttribute
    {
        /// <summary>
        /// Maps the variable assignments to the property names.
        /// </summary>
        public Dictionary<string, string> VariableAssignments { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Map of the names of properties that are attributed as Parameter to their corresponding IParameterModel instances.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, IParameterModel> ParameterMap { get; } = new Dictionary<string, IParameterModel>();

        /// <summary>
        /// Call this if you do not implement the protected ctor.
        /// </summary>
        /// <param name="parameterModelFactory"></param>
        public void InitializeAttributeMap(IParameterModelFactory parameterModelFactory)
        {
            if (ParameterMap.Count == 0)
            {
                Dictionary<string, IParameterModel> models = parameterModelFactory.GetModels(this);
                foreach (var model in models)
                {
                    ParameterMap.Add(model.Key, model.Value);
                }
            }
        }

        /// <summary>
        /// Use this ctor if you want to use the default ParameterModelFactory.
        /// </summary>
        /// <param name="parameterModelFactory"></param>
        protected ImplementsParametersBase() : this(new ParameterModelFactory()) { }

        /// <summary>
        /// Use this ctor if you want to inject your own IParameterModelFactory.
        /// </summary>
        /// <param name="parameterModelFactory"></param>
        protected ImplementsParametersBase(IParameterModelFactory parameterModelFactory) 
        {
            InitializeAttributeMap(parameterModelFactory);
        }
    }
}
