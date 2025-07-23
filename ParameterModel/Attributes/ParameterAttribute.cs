using ParameterModel.Extensions;
using ParameterModel.Interfaces;
using ParameterModel.Variables;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ParameterModel.Attributes
{
    /// <summary>
    /// Holds PropertyInfo and parent class which implements IImplementsParameterAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ParameterAttribute : Attribute
    {
        private Array _enumValues;
        protected Dictionary<int, string> _enumIntDisplayDict;
        protected Dictionary<Enum, string> _enumItemsDisplayDict;
        public IImplementsParameterAttribute ImplementsParameterAttributes { get; protected set; }

        /// <summary>
        /// Used in a prompt.
        /// </summary>
        public string Prompt { get; protected set; } = "";

        /// <summary>
        /// Optional.
        /// </summary>
        public string Description { get; protected set; } = "";

        ///// <summary>
        ///// Optional.
        ///// </summary>
        //public string Units { get; set; } = "";

        /// <summary>
        /// If true, then this property can be a variable. 
        /// This is used to indicate that the value can be set by a variable in the context of the application.
        /// This meas there will be an entry in a VariableAssignments property to resolve.
        /// </summary>
        public bool CanBeVariable { get; protected set; } = false;

        /// <summary>
        /// Use ths to implement an order to how these appear in a dialog or form.
        /// Lower numbers appear first, optional;
        /// </summary>
        public int PresentationOrder { get; set; } = 5;

        public List<string> ValidationErrors { get; } = new List<string>();

        public bool IsReadOnly { get; protected set; }

        ///// <summary>
        ///// If the value can be a variable, then this is the type of the evaluation result.
        ///// This can only ever be applied to a property type of string.
        ///// </summary>
        //public Type VariableType { get; set; }

        public PropertyInfo PropertyInfo { get; protected set; }

        public ParameterAttribute(bool canBeVariable) 
        {
            CanBeVariable = canBeVariable;
        }

        public ParameterAttribute() : this(false) { }

        //public ParameterAttribute(string label)
        //{ 
        //    Label = label;
        //}

        public Dictionary<Enum, string> GetEnumItemsDisplay()
        {
            return _enumItemsDisplayDict;
        }
        public bool IsVariableSelected { get => ImplementsParameterAttributes.VariableAssignments.ContainsKey(PropertyInfo.Name); }

        public bool GetDisplayString(out string displayString, out bool isVariableAssignment)
        {
            return ImplementsParameterAttributes.GetDisplayString(PropertyInfo.Name, out displayString, out isVariableAssignment);
        }

        public bool TrySetPropertyValue(string newValue, out string error)
        {
            return ImplementsParameterAttributes.TrySetPropertyValue(PropertyInfo.Name, newValue, out error);
        }

        public bool TrySetVariableValue(string newValue, out string error)
        {
            return ImplementsParameterAttributes.TryAssignVariable(PropertyInfo.Name, newValue, out error);
        }

        public bool TestPropertyValue(string newValue, out string error)
        {
            return ImplementsParameterAttributes.TestPropertyValue(PropertyInfo.Name, newValue, out error);
        }

        public static bool TestAllowedValidationAttributes(PropertyInfo propertyInfo, List<string> invalidAttributeNames)
        {
            invalidAttributeNames.Clear();
            Type type = propertyInfo.PropertyType;
            List<Type> atts = propertyInfo.GetCustomAttributes<ValidationAttribute>().Select(s => s.GetType()).ToList();
            List<Type> allowed = ParameterAttribute.AllowedValidationAttributes[type];
            foreach (var attribute in atts)
            {
                if (!allowed.Contains(attribute))
                {
                    invalidAttributeNames.Add(attribute.Name);
                }
            }
            return invalidAttributeNames.Count == 0;
        }

        public static void SetPropertyInfo(ParameterAttribute parameterAttribute, PropertyInfo propertyInfo,
            IImplementsParameterAttribute implements)
        {
            if(parameterAttribute.PropertyInfo == null)
            {
                parameterAttribute.PropertyInfo = propertyInfo;
                parameterAttribute.ImplementsParameterAttributes = implements;
                EditableAttribute editableAttribute = parameterAttribute.PropertyInfo.GetCustomAttribute<EditableAttribute>();
                parameterAttribute.IsReadOnly = (editableAttribute != null && !editableAttribute.AllowEdit) ||
                                                parameterAttribute.PropertyInfo.GetCustomAttribute<ReadOnlyAttribute>()?.IsReadOnly == true;
                DisplayAttribute displayAttribute = parameterAttribute.PropertyInfo.GetCustomAttribute<DisplayAttribute>();
                if(displayAttribute != null)
                {
                    parameterAttribute.Prompt = displayAttribute.Prompt;
                    parameterAttribute.PresentationOrder = displayAttribute.GetOrder() ?? 5; // Default to 5 if not set
                    parameterAttribute.Description = displayAttribute.Description ?? ""; // Default to empty if not set
                }
                else
                {
                    parameterAttribute.Prompt = propertyInfo.Name; // Default label to property name if not set
                    parameterAttribute.PresentationOrder = 5; // Default order
                    parameterAttribute.Description = null; // Default to empty if not set
                }
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
                parameterAttribute._enumItemsDisplayDict =
                    parameterAttribute._enumValues.Cast<Enum>().ToDictionary(s => s, s => EnumToDescriptionOrString(s));
                parameterAttribute._enumIntDisplayDict = 
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

        public static Dictionary<Type, List<Type>> AllowedValidationAttributes { get; } = new Dictionary<Type, List<Type>>()
        {
            { 
                typeof(int), new List<Type>()
                {
                    //typeof(RequiredAttribute),
                    //typeof(StringLengthAttribute),
                    typeof(RangeAttribute),
                    //typeof(EmailAddressAttribute),
                    //typeof(PhoneAttribute),
                    //typeof(UrlAttribute),
                    //typeof(DataTypeAttribute),
                    //typeof(FileExtensionsAttribute),
                    typeof(DisplayAttribute),
                    typeof(DisplayFormatAttribute),
                    typeof(EditableAttribute),
                }
            },
            { 
                typeof(float), new List<Type>()
                {
                    //typeof(RequiredAttribute),
                    //typeof(StringLengthAttribute),
                    typeof(RangeAttribute),
                    //typeof(EmailAddressAttribute),
                    //typeof(PhoneAttribute),
                    //typeof(UrlAttribute),
                    //typeof(DataTypeAttribute),
                    //typeof(FileExtensionsAttribute),
                    //typeof(CustomValidationAttribute),
                    typeof(DisplayAttribute),
                    typeof(DisplayFormatAttribute),
                    typeof(EditableAttribute),
                }
            },
            { 
                typeof(string), new List<Type>()
                {
                    typeof(RequiredAttribute),
                    typeof(StringLengthAttribute),
                    //typeof(RangeAttribute),
                    typeof(EmailAddressAttribute),
                    typeof(PhoneAttribute),
                    typeof(UrlAttribute),
                    //typeof(DataTypeAttribute),
                    typeof(FileExtensionsAttribute),
                    //typeof(CustomValidationAttribute),
                    typeof(DisplayAttribute),
                    //typeof(DisplayFormatAttribute),
                    typeof(EditableAttribute),
                }
            },
            { 
                typeof(Enum), new List<Type>()
                {
                    //typeof(RequiredAttribute),
                    //typeof(StringLengthAttribute),
                    //typeof(RangeAttribute),
                    //typeof(EmailAddressAttribute),
                    //typeof(PhoneAttribute),
                    //typeof(UrlAttribute),
                    //typeof(DataTypeAttribute),
                    //typeof(FileExtensionsAttribute),
                    //typeof(CustomValidationAttribute),
                    typeof(DisplayAttribute),
                    //typeof(DisplayFormatAttribute),
                    typeof(EditableAttribute),
                }
            },
            {
                typeof(string[]), new List<Type>()
                {
                    typeof(RequiredAttribute),
                    //typeof(StringLengthAttribute),
                    //typeof(RangeAttribute),
                    //typeof(EmailAddressAttribute),
                    //typeof(PhoneAttribute),
                    //typeof(UrlAttribute),
                    //typeof(DataTypeAttribute),
                    //typeof(FileExtensionsAttribute),
                    //typeof(CustomValidationAttribute),
                    typeof(DisplayAttribute),
                    //typeof(DisplayFormatAttribute),
                    typeof(EditableAttribute),
                }
            },
            { 
                typeof(bool), new List<Type>()
                {
                    //typeof(RequiredAttribute),
                    //typeof(StringLengthAttribute),
                    //typeof(RangeAttribute),
                    //typeof(EmailAddressAttribute),
                    //typeof(PhoneAttribute),
                    //typeof(UrlAttribute),
                    //typeof(DataTypeAttribute),
                    //typeof(FileExtensionsAttribute),
                    //typeof(CustomValidationAttribute),
                    typeof(DisplayAttribute),
                    //typeof(DisplayFormatAttribute),
                    typeof(EditableAttribute),
                }
            },
            {
                typeof(Variable), new List<Type>()
                {
                    typeof(RequiredAttribute),
                    typeof(DisplayAttribute),
                    typeof(EditableAttribute),
                }
            }
        };
    }
}
