using ParameterModel.Attributes;
using ParameterModel.Extensions;
using ParameterModel.Interfaces;
using ParameterModel.Models;
using ParameterModel.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ParameterModel.Factories
{
    public class ParameterModelFactory
    {
        private readonly IVariablesContext _variablesContext;
        public ParameterModelFactory(IVariablesContext variablesContext) 
        { 
            _variablesContext = variablesContext;
        }

        public Dictionary<string, IParameterModel> GetModels(IImplementsParameterAttribute propertyOwner)
        {
            Dictionary<string, ParameterAttribute> attributeMap = propertyOwner.GetAttributeMap();
            Dictionary<string, IParameterModel> ret = new Dictionary<string, IParameterModel>();
            foreach (var kvp in attributeMap)
            {
                //PropertyInfo propertyInfo = kvp.Value.PropertyInfo;
                //ParameterAttribute parameterPromptAttribute = kvp.Value;
                IParameterModel parameterModel = null;
                Type type = kvp.Value.PropertyInfo.PropertyType;
                //if (kvp.Value.PropertyInfo.PropertyType != null)
                //{
                //    type = parameterPromptAttribute.VariableType;
                //}
                //else
                //{
                //    type = kvp.Value.PropertyInfo.PropertyType;
                //}

                //#region 
                //protected RequiredAttribute requiredAttribute; //Ensures that a property cannot be null or empty.It's commonly used to indicate mandatory fields. 
                //protected StringLengthAttribute stringLengthAttribute; //Specifies the minimum and maximum length of a string property. 
                //protected RangeAttribute rangeAttribute; //Limits a numeric property to a specific range of values. 
                //protected EmailAddressAttribute emailAddressAttribute; //Validates that a string property is a valid email address. 
                //protected PhoneAttribute phoneAttribute; //Validates that a string property is a valid phone number. 
                //protected UrlAttribute urlAttribute; //Validates that a string property is a valid URL.
                //protected DataTypeAttribute dataTypeAttribute; //Provides metadata about the data type of a property, such as Date, Time, PhoneNumber, Currency, etc.
                //protected EnumDataTypeAttribute enumDataTypeAttribute; //Links an enum to a data column, ensuring the property's value is within the enum's range.
                //protected FileExtensionsAttribute fileExtensionsAttribute; //Validates that a file name extension is valid.
                //protected CustomValidationAttribute customValidationAttribute; //Allows for custom validation logic to be applied to a property.
                //protected DisplayAttribute displayAttribute; // Specifies localizable strings for display purposes, such as the field's name, description, or prompt.
                //protected DisplayFormatAttribute displayFormatAttribute; // Controls how data fields are displayed and formatted in user interfaces.
                //protected EditableAttribute editableAttribute; // Indicates whether a property is editable or read-only
                //#endregion

                // Indicates whether a property is editable or read-only
                EditableAttribute editableAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<EditableAttribute>();
                //Ensures that a property cannot be null or empty.It's commonly used to indicate mandatory fields.
                RequiredAttribute requiredAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<RequiredAttribute>();
                //Specifies the minimum and maximum length of a string property.
                StringLengthAttribute stringLengthAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<StringLengthAttribute>();
                //Limits a numeric property to a specific range of values.
                RangeAttribute rangeAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<RangeAttribute>();
                //Validates that a string property is a valid email address. 
                EmailAddressAttribute emailAddressAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<EmailAddressAttribute>();
                //Validates that a string property is a valid phone number. 
                PhoneAttribute phoneAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<PhoneAttribute>();
                //Validates that a string property is a valid URL.
                UrlAttribute urlAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<UrlAttribute>();
                //Provides metadata about the data type of a property, such as Date, Time, PhoneNumber, Currency, etc.
                DataTypeAttribute dataTypeAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<DataTypeAttribute>();
                //Links an enum to a data column, ensuring the property's value is within the enum's range.
                EnumDataTypeAttribute enumDataTypeAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<EnumDataTypeAttribute>();
                //Validates that a file name extension is valid.
                FileExtensionsAttribute fileExtensionsAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<FileExtensionsAttribute>();
                //Allows for custom validation logic to be applied to a property.
                CustomValidationAttribute customValidationAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<CustomValidationAttribute>();
                // Specifies localizable strings for display purposes, such as the field's name, description, or prompt.
                DisplayAttribute displayAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<DisplayAttribute>();
                // Controls how data fields are displayed and formatted in user interfaces.
                DisplayFormatAttribute displayFormatAttribute = kvp.Value.PropertyInfo.GetCustomAttribute<DisplayFormatAttribute>();

                if (type == typeof(bool))
                {
                    ExceptionForInvalidAttribute(type, [requiredAttribute, stringLengthAttribute, 
                        rangeAttribute, emailAddressAttribute, phoneAttribute, urlAttribute, dataTypeAttribute, enumDataTypeAttribute, fileExtensionsAttribute,
                        customValidationAttribute, displayAttribute, displayFormatAttribute]);
                    parameterModel = new BoolParameterModel(kvp.Value, _variablesContext);
                }
                else if (type.IsEnum && (type == typeof(Enum)))
                {
                    ExceptionForInvalidAttribute(type, [requiredAttribute, stringLengthAttribute,
                        rangeAttribute, emailAddressAttribute, phoneAttribute, urlAttribute, dataTypeAttribute, enumDataTypeAttribute, fileExtensionsAttribute,
                        customValidationAttribute, displayAttribute, displayFormatAttribute]);
                    parameterModel = new EnumParameterModel(kvp.Value, _variablesContext);
                }
                else if (type == typeof(string))
                {
                    ExceptionForInvalidAttribute(type, [rangeAttribute, enumDataTypeAttribute, customValidationAttribute, displayAttribute, displayFormatAttribute]);
                    parameterModel = new StringParameterModel(kvp.Value, _variablesContext);
                }
                else if (type == typeof(int))
                {
                    ExceptionForInvalidAttribute(type, [requiredAttribute, stringLengthAttribute, emailAddressAttribute, phoneAttribute, 
                        urlAttribute, dataTypeAttribute, enumDataTypeAttribute, fileExtensionsAttribute,
                        customValidationAttribute, displayAttribute]);
                    parameterModel = new IntParameterModel(kvp.Value, _variablesContext);
                }
                else if (type == typeof(float))
                {
                    ExceptionForInvalidAttribute(type, [requiredAttribute, stringLengthAttribute, emailAddressAttribute, phoneAttribute,
                        urlAttribute, dataTypeAttribute, enumDataTypeAttribute, fileExtensionsAttribute,
                        customValidationAttribute, displayAttribute]);
                    parameterModel = new FloatParameterModel(kvp.Value, _variablesContext);
                }
                else if (type == typeof(string[]))
                {
                    ExceptionForInvalidAttribute(type, [stringLengthAttribute,
                        rangeAttribute, emailAddressAttribute, phoneAttribute, urlAttribute, dataTypeAttribute, enumDataTypeAttribute, fileExtensionsAttribute,
                        customValidationAttribute, displayAttribute, displayFormatAttribute]);
                }
                else
                {
                    throw new NotSupportedException($"Type {type} is not supported.");
                }
                ret.Add(kvp.Value.PropertyInfo.Name, parameterModel);
            }
            return ret;
        }

        private void ExceptionForInvalidAttribute(Type type, ICollection<Attribute> attributes)
        {
            List<string> attributeNames = new List<string>();
            foreach (var attribute in attributes)
            {
                if(attribute != null)
                {
                    attributeNames.Add(attribute.GetType().Name);
                }
            }
            if (attributeNames.Count != 0)
            {
                throw new NotSupportedException($"Parameter type {type} does not support these attribute(s): " + string.Join(", ", attributeNames));
            }
        }
    }
}
