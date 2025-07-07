using ParameterModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public VariableBase(string name, VariableType type) 
        { 
            Name = name;
            Type = type;
        }

        public string Name { get; set; } = string.Empty;

        public VariableType Type { get; } = VariableType.Undefined;

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
            if (Type == VariableType.Boolean && bool.TryParse(_stringValue, out bool b))
            {
                return b;
            }
            throw new InvalidOperationException("Variable type is not Boolean.");
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
            _numericValue = newValue;
        }

        public void SetValue(string newValue)
        {
            _stringValue = newValue;
        }

        public void SetValue(bool newValue)
        {
            _stringValue = newValue.ToString();
        }

        public void SetValue(float newValue)
        {
            _numericValue = newValue;
        }

    }
}
