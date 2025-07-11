using ParameterModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace ParameterModel.Variables
{
 

    public class VariableBase
    {
        /// <summary>
        /// Use for int or float.
        /// </summary>
        private double _numericValue = 0.0;
        /// <summary>
        /// Use for string, JSON, and boolean values.
        /// </summary>
        private string _stringValue = string.Empty;

        private bool _boolValue = false;
        public VariableBase(string name, VariableType type) 
        { 
            Name = name;
            Type = type;
        }

        public string Name { get; set; } = string.Empty;

        public VariableType Type { get; } = VariableType.Undefined;

        public bool IsReadOnly { get; set; } = false;

        public int GetValueAsInt()
        {
            if (Type == VariableType.Integer)
            {
                return (int)_numericValue;
            }
            throw new InvalidOperationException("Variable type is not Integer.");
        }

        public string GetValueAsString()
        {
            if (Type == VariableType.String)
            {
                return _stringValue;
            }
            else if (Type == VariableType.JSON)
            {
                return _stringValue; // JSON is stored as a string
            }
            else if (Type == VariableType.Boolean)
            {
                return _boolValue.ToString(); // Boolean is stored as a string representation
            }
            else if (Type == VariableType.Float)
            {
                return ((float)_numericValue).ToString(); // Float is stored as a string representation
            }
            else if (Type == VariableType.Integer)
            {
                return ((int)_numericValue).ToString(); // Undefined type returns empty string
            }
            throw new InvalidOperationException("Variable type is not String.");
        }

        public string GetValueAsJson() 
        {
            if (Type == VariableType.JSON)
            {
                return _stringValue;
            }
            throw new InvalidOperationException("Variable type is not JSON.");
        }

        public bool GetValueAsBool() 
        {
            if (Type == VariableType.Boolean)
            {
                return _boolValue;
            }
            throw new InvalidOperationException("Variable type is not Boolean.");
        }

        public T GetValueAs<T>()
        {
            if (Type == VariableType.Integer && typeof(T) == typeof(int))
            {
                return (T)(object)GetValueAsInt();
            }
            else if (Type == VariableType.Float && typeof(T) == typeof(float))
            {
                return (T)(object)GetValueAsFloat();
            }
            else if (Type == VariableType.Boolean && typeof(T) == typeof(bool))
            {
                return (T)(object)GetValueAsBool();
            }
            else if ( ((Type == VariableType.String) || (Type == VariableType.JSON)) && 
                (typeof(T) == typeof(string)))
            {
                return (T)(object)GetValueAsString();
            }
            
            throw new InvalidOperationException($"Variable type {Type} is not compatible with requested type {typeof(T)}.");
        }

        public float GetValueAsFloat()
        {
            if (Type == VariableType.Float)
            {
                return (float)_numericValue;
            }
            throw new InvalidOperationException("Variable type is not Float.");
        }

        //public void GetValue<T>(out T value)
        //{
        //    if (Type == VariableType.Float)
        //    {
        //        value = GetValueAsFloat();
        //    }
        //    throw new InvalidOperationException("Variable type is not Float.");
        //}

        public void SetValue(int newValue)
        {
            if ((Type != VariableType.Integer) && (Type != VariableType.Float))
            {
                throw new InvalidOperationException("Variable type is not Integer or Float.");
            }
            _numericValue = newValue;
        }

        public void SetValue(string newValue)
        {
            if (Type == VariableType.String)// && Type != VariableType.JSON)
            {
                _stringValue = newValue;
            }
            else if (Type == VariableType.JSON)
            {
                if(!IsJson(newValue))
                {
                    throw new ArgumentException("Provided string is not valid JSON.", nameof(newValue));
                }
                _stringValue = newValue;
            }
            throw new InvalidOperationException("Variable type is not String or JSON.");
        }

        public void SetValue(bool newValue)
        {
            if (Type != VariableType.Boolean)
            {
                throw new InvalidOperationException("Variable type is not Boolean.");
            }
            _boolValue = newValue;
        }

        public void SetValue(float newValue)
        {
            if (Type != VariableType.Float)
            {
                throw new InvalidOperationException("Variable type is not Float.");
            }
            _numericValue = newValue;
        }

        public static bool IsJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            try
            {
                // Convert string to UTF-8 bytes
                var utf8Bytes = Encoding.UTF8.GetBytes(input);
                var reader = new Utf8JsonReader(utf8Bytes);

                if (!reader.Read() || !reader.TrySkip())
                {
                    return false;
                }

                // Make sure there's nothing after the JSON
                return !reader.Read();
            }
            catch
            {
                return false;
            }
        }
    }
}
