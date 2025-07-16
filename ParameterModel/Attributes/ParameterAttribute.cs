using ParameterModel.Extensions;
using ParameterModel.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ParameterModel.Attributes
{
    /// <summary>
    /// Holds PropertyInfo and parent class which implements IImplementsParameterAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ParameterAttribute : Attribute
    {
        private Array _enumValues;
        public Dictionary<int, string> EnumIntDisplayDict { get; protected set; }
        public Dictionary<Enum, string> EnumItemsDisplayDict { get; protected set; }

        protected IImplementsParameterAttribute _implementsParameterAttribute;

        /// <summary>
        /// Used in a prompt.
        /// </summary>
        public string Label { get; set; } = "";
        /// <summary>
        /// Optional.
        /// </summary>
        public string Units { get; set; } = "";
        ///// <summary>
        ///// If localized application, optional. "" value to be ignorred.
        ///// </summary>
        //public string LanguageKey { get; set; } = "";
        /// <summary>
        /// Use ths to implement an order to how these appear in a dialog or form.
        /// Lower numbers appear first, optional;
        /// </summary>
        public int PresentationOrder { get; set; } = 5;

        public List<string> ValidationErrors { get; } = new List<string>();

        public bool IsReadOnly { get; protected set; }

        /// <summary>
        /// Optional.
        /// </summary>
        public string ToolTipNotes { get; set; } = "";

        /// <summary>
        /// If true, then this property can be a variable. 
        /// This is used to indicate that the value can be set by a variable in the context of the application.
        /// This meas there will be an entry in a VariableAssignments property to resolve.
        /// </summary>
        public bool CanBeVariable { get; set; } = false; 

        ///// <summary>
        ///// If the value can be a variable, then this is the type of the evaluation result.
        ///// This can only ever be applied to a property type of string.
        ///// </summary>
        //public Type VariableType { get; set; }

        public PropertyInfo PropertyInfo { get; protected set; }

        public ParameterAttribute() { }

        //public ParameterAttribute(string label)
        //{ 
        //    Label = label;
        //}

        public bool IsVariableSelected { get => _implementsParameterAttribute.VariableAssignments.ContainsKey(PropertyInfo.Name); }

        public bool GetDisplayString(out string displayString, out bool isVariableAssignment)
        {
            return _implementsParameterAttribute.GetDisplayString(PropertyInfo.Name, out displayString, out isVariableAssignment);
        }

        public bool TrySetPropertyValue(string newValue, out string error)
        {
            return _implementsParameterAttribute.TrySetPropertyValue(PropertyInfo.Name, newValue, out error);
        }

        public bool TrySetVariableValue(string newValue, out string error)
        {
            return _implementsParameterAttribute.TryAssignVariable(PropertyInfo.Name, newValue, out error);
        }

        public bool TestPropertyValue(string newValue, out string error)
        {
            return _implementsParameterAttribute.TestPropertyValue(PropertyInfo.Name, newValue, out error);
        }


        public static void SetPropertyInfo(ParameterAttribute parameterAttribute, PropertyInfo propertyInfo,
            IImplementsParameterAttribute implements)
        {
            if(parameterAttribute.PropertyInfo == null)
            {
                parameterAttribute.PropertyInfo = propertyInfo;
                parameterAttribute._implementsParameterAttribute = implements;
                EditableAttribute editableAttribute = parameterAttribute.PropertyInfo.GetCustomAttribute<EditableAttribute>();

                parameterAttribute.IsReadOnly = (editableAttribute != null && !editableAttribute.AllowEdit) ||
                                                parameterAttribute.PropertyInfo.GetCustomAttribute<ReadOnlyAttribute>()?.IsReadOnly == true;
            }
        }

        public static void InitEnumData(ParameterAttribute parameterAttribute)
        {
            if (parameterAttribute.PropertyInfo.PropertyType.IsEnum)
            {
                parameterAttribute._enumValues = Enum.GetValues(parameterAttribute.PropertyInfo.PropertyType);//);.EnumType);
                if (parameterAttribute._enumValues.Length == 0)
                {
                    throw new ArgumentException($"Enum type {parameterAttribute.PropertyInfo.PropertyType} does not contain any values.", nameof(parameterAttribute.PropertyInfo.Name));
                }
                parameterAttribute.EnumItemsDisplayDict =
                    parameterAttribute._enumValues.Cast<Enum>().ToDictionary(s => s, s => EnumToDescriptionOrString(s));
                parameterAttribute.EnumIntDisplayDict = 
                    parameterAttribute._enumValues.Cast<Enum>().ToDictionary(e => Convert.ToInt32(e), e => EnumToDescriptionOrString(e));
            }
        }

        private static string EnumToDescriptionOrString(Enum value)
        {
            return value.GetType().GetField(value.ToString())
                       .GetCustomAttributes(typeof(DescriptionAttribute), false)
                       .Cast<DescriptionAttribute>()
                       .FirstOrDefault()?.Description ?? value.ToString();
        }

        //#region 
        //protected RequiredAttribute _requiredAttribute; //Ensures that a property cannot be null or empty.It's commonly used to indicate mandatory fields. 
        //protected StringLengthAttribute _stringLengthAttribute; //Specifies the minimum and maximum length of a string property. 
        //protected RangeAttribute _rangeAttribute; //Limits a numeric property to a specific range of values. 
        //protected EmailAddressAttribute _emailAddressAttribute; //Validates that a string property is a valid email address. 
        //protected PhoneAttribute _phoneAttribute; //Validates that a string property is a valid phone number. 
        //protected UrlAttribute _urlAttribute; //Validates that a string property is a valid URL.
        //protected DataTypeAttribute _dataTypeAttribute; //Provides metadata about the data type of a property, such as Date, Time, PhoneNumber, Currency, etc.
        //protected EnumDataTypeAttribute _enumDataTypeAttribute; //Links an enum to a data column, ensuring the property's value is within the enum's range.
        //protected FileExtensionsAttribute _fileExtensionsAttribute; //Validates that a file name extension is valid.
        //protected CustomValidationAttribute _customValidationAttribute; //Allows for custom validation logic to be applied to a property.
        //protected DisplayAttribute _displayAttribute; // Specifies localizable strings for display purposes, such as the field's name, description, or prompt.
        //protected DisplayFormatAttribute _displayFormatAttribute; // Controls how data fields are displayed and formatted in user interfaces.
        //protected EditableAttribute _editableAttribute; // Indicates whether a property is editable or read-only
        //#endregion

    }
}
