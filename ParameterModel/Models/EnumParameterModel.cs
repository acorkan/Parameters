using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Models
{
    public class EnumParameterModel : ParameterModelBase
    {
        //private Array _enumValues;
        ////private int[] _intValues;
        //private readonly Dictionary<int, string> _values;
        //private readonly Dictionary<Enum, string> _enumItemsDisplaySource;

        private readonly string[] _selections;

        public EnumParameterModel(ParameterAttribute parameterPromptAttribute, IVariablesContext variablesContext) : 
            base(parameterPromptAttribute, variablesContext)
        {
            _selections = parameterPromptAttribute.EnumItemsDisplayDict.Values.ToArray();
        }

        public override string[] GetSelectionItems()
        {
            return _selections;
        }
    }
}
