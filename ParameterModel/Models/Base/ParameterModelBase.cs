using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Variables;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ParameterModel.Models.Base
{
    /// <summary>
    /// This model is for each property that has a ParameterAttribute and will appear in a prompt.
    /// 
    /// If ParameterAttribute.CanBeVariable is set and VariablesContext is not null then the property can be assigned from a variable as well.
    /// </summary>
    public abstract class ParameterModelBase : ModelBase<ParameterModelMessage>, IParameterModel
    {
        public ParameterModelBase(ParameterAttribute parameterPromptAttribute)
        {
            ParameterAttribute = parameterPromptAttribute;
        }


        #region IParameterModel
        public ParameterAttribute ParameterAttribute { get; }
        public bool CanBeVariable { get => ParameterAttribute.CanBeVariable; }
        public bool IsReadOnly { get => ParameterAttribute.IsReadOnly; }
        public bool HasError { get => Errors.Count != 0; }
        public Type ParameterType { get => ParameterAttribute.PropertyInfo.PropertyType; }
        public string ParameterName { get => ParameterAttribute.PropertyInfo.Name; }
        public List<string> Errors { get; } = new List<string>();
        public bool IsVariableSelected { get => ParameterAttribute.IsVariableSelected; }

        /// <summary>
        /// This returns selections that can be used in a combobox.
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetSelectionItems() => Array.Empty<string>(); // Default implementation returns an empty array.

        /// <summary>
        /// Return true if the variable assingment can be made.
        /// If implements is not null then the variable will be set to the value if valid.
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="errorMessage"></param>
        /// <param name="setVarValue"></param>
        /// <returns></returns>
        public virtual bool TestOrAssignVariable(IVariablesContext variablesContext, string varName, bool setVarValue)
        {
            if (!ParameterAttribute.CanBeVariable)
            {
                throw new InvalidOperationException($"Property '{ParameterName}' is not marked as a variable assignment.");
            }
            else if (variablesContext == null)
            {
                throw new InvalidOperationException($"VariablesContext cannot be null because property {ParameterAttribute.PropertyInfo.Name} has CanBeVariable set.");
            }

            if (ParameterAttribute.IsReadOnly)
            {
                throw new InvalidOperationException($"Parameter {ParameterName} is read-only.");
            }

            if (string.IsNullOrEmpty(varName))
            {
                throw new ArgumentNullException("Variable name cannot be null or empty.");
            }

            VariableBase variable = variablesContext.GetVariable(varName);
            if ((variable == null) || !AllowedVariableTypes.Contains(variable.Type))
            {
                return false;
            }

            if (setVarValue)
            {
                // OK to assign variable.
                ParameterAttribute.ImplementsParameterAttributes.VariableAssignments[ParameterName] = varName;
            }
            return true;
        }

        /// <summary>
        /// Return true if the property value can be set from the newValue string.
        /// If implements is not null then the property will be set to the newValue if valid.
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="errorMessage"></param>
        /// <param name="implements"></param>
        /// <returns></returns>
        public abstract bool TestOrSetParameter(string newValue, bool setProperty);

        public string GetDisplayString(out bool isVariableAssignment)
        {
            //return ParameterAttribute.GetDisplayString(out displayString, out isVariableAssignment);
            isVariableAssignment = false;
            if (ParameterAttribute.ImplementsParameterAttributes.VariableAssignments.ContainsKey(ParameterName))
            {
                // If the variable is set, then return that value.
                isVariableAssignment = true;
                return ParameterAttribute.ImplementsParameterAttributes.VariableAssignments[ParameterName];
            }
            return GetDisplayString();
        }

        /// <summary>
        /// Just format and return the proeprty value as a string.
        /// Ignore any possible variable assignment.
        /// </summary>
        /// <param name="implements"></param>
        /// <returns></returns>
        protected abstract string GetDisplayString();

        public abstract VariableType[] AllowedVariableTypes { get; }

        public bool ValidateParameter(List<string> errors)
        {
            errors.Clear();
            PropertyInfo propertyInfo = ParameterAttribute.PropertyInfo;
            var results = new List<ValidationResult>();
            var context = new ValidationContext(ParameterAttribute.ImplementsParameterAttributes) { MemberName = propertyInfo.Name };
            object value = propertyInfo.GetValue(ParameterAttribute.ImplementsParameterAttributes);
            Validator.TryValidateProperty(value, context, results);
            if (results.Any())
            {
                errors.AddRange(results.Select(r => r.ErrorMessage));
            }
            return !errors.Any();
        }

        #endregion IParameterModel
    }


    public class ParameterModelMessage : ModelDependentMessage 
    {
    }
}
