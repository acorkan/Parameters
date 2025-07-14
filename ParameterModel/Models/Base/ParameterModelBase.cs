using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Variables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParameterModel.Models.Base
{
    /// <summary>
    /// This model is for each property that has a ParameterAttribute and will appear in a prompt.
    /// 
    /// If ParameterAttribute.CanBeVariable is set and VariablesContext is not null then the property can be assigned from a variable as well.
    /// </summary>
    public abstract class ParameterModelBase : ModelBase<ParameterModelMessage>, IParameterModel
    {
        //protected IImplementsParameterAttribute _propertyOwner;

        public ParameterModelBase(ParameterAttribute parameterPromptAttribute,
            //IImplementsParameterAttribute propertyOwner,
            IVariablesContext variablesContext)
        {
            ParameterAttribute = parameterPromptAttribute;
            //_propertyOwner = propertyOwner;
            VariablesContext = variablesContext;
            if (CanBeVariable && (VariablesContext == null))
            {
                throw new InvalidOperationException($"VariablesContext cannot be null because property {ParameterAttribute.PropertyInfo.Name} has CanBeVariable set.");
            }
        }


        #region IParameterModel
        public ParameterAttribute ParameterAttribute { get; }
        public IVariablesContext VariablesContext { get; }
        public bool CanBeVariable { get => ParameterAttribute.CanBeVariable; }
        public bool HasError { get => Errors.Count != 0; }
        public Type ParameterType { get => ParameterAttribute.PropertyInfo.PropertyType; }
        public string ParameterName { get => ParameterAttribute.PropertyInfo.Name; }
        public List<string> Errors { get; } = new List<string>();
        public bool IsVariableSelected { get => ParameterAttribute.IsVariableSelected; }

        public bool TestValueString(string valueString)
        {
            return ParameterAttribute.TestPropertyValue(valueString, out string errorMessage);
        }

        public void AssignValueStringToProperty(string valueString)
        {
            ParameterAttribute.TrySetPropertyValue(valueString, out string errorMessage);
        }

        public void AssignVaribleToProperty(string valueString)
        {
            ParameterAttribute.TrySetVariableValue(valueString, out string errorMessage);
        }

        public bool GetDisplayString(out string displayString, out bool isVariableAssignment)
        {
            return ParameterAttribute.GetDisplayString(out displayString, out isVariableAssignment);
        }

        public string GetDisplayString()
        {
            ParameterAttribute.GetDisplayString(out string displayString, out bool isVariableAssignment);
            return displayString;
        }

        /// <summary>
        /// This returns selections that can be used in a combobox.
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetSelectionItems() => Array.Empty<string>(); // Default implementation returns an empty array.
        #endregion IParameterModel
    }


    public class ParameterModelMessage : ModelDependentMessage 
    {
    }
}
