using ParameterModel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Interfaces
{
    public interface IParameterModel
    {
        ParameterAttribute ParameterAttribute { get; }
        PropertyInfo PropertyInfo { get; }

        //bool TryParse(string valueString, out T value);
        bool Validate(List<string> errors);

        string Format();

        //void SetPropertyValue(T newValue);

        string GetPropertyValueString();

        string[] GetSelections();

        V GetValue<V>();

        void SetPropertyValue<V>(V newValue);
    }
}
