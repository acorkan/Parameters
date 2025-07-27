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

        public bool IsReadOnly { get; protected set; }

        public PropertyInfo PropertyInfo { get; protected set; }

        public ParameterAttribute(bool canBeVariable) 
        {
            CanBeVariable = canBeVariable;
        }

        public ParameterAttribute() : this(false) { }


        public Dictionary<Enum, string> GetEnumItemsDisplay()
        {
            return _enumItemsDisplayDict;
        }
        public bool IsVariableSelected { get => ImplementsParameterAttributes.VariableAssignments.ContainsKey(PropertyInfo.Name); }


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
                    typeof(RangeAttribute),
                    typeof(DisplayAttribute),
                    typeof(DisplayFormatAttribute),
                    typeof(EditableAttribute),
                    //typeof(CustomValidationAttribute),
                }
            },
            { 
                typeof(float), new List<Type>()
                {
                    typeof(RangeAttribute),
                    typeof(DisplayAttribute),
                    typeof(DisplayFormatAttribute),
                    typeof(EditableAttribute),
                    //typeof(CustomValidationAttribute),
                }
            },
            { 
                typeof(string), new List<Type>()
                {
                    typeof(StringLengthAttribute),
                    typeof(DisplayAttribute),
                    typeof(EditableAttribute),
                    //typeof(CustomValidationAttribute),
                }
            },
            { 
                typeof(Enum), new List<Type>()
                {
                    typeof(DisplayAttribute),
                    typeof(EditableAttribute),
                    //typeof(CustomValidationAttribute),
                }
            },
            {
                typeof(string[]), new List<Type>()
                {
                    typeof(DisplayAttribute),
                    typeof(EditableAttribute),
                    //typeof(CustomValidationAttribute),
                }
            },
            { 
                typeof(bool), new List<Type>()
                {
                    typeof(DisplayAttribute),
                    typeof(EditableAttribute),
                    //typeof(CustomValidationAttribute),
                }
            },
            {
                typeof(VariableProperty), new List<Type>()
                {
                    typeof(DisplayAttribute),
                    typeof(EditableAttribute),
                }
            }
        };
    }
}
