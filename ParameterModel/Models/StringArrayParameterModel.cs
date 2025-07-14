using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Models
{
    public class StringArrayParameterModel : ParameterModelBase
    {
        public static readonly char StrArrayParameterDelimiter = ',';

        public StringArrayParameterModel(ParameterAttribute parameterPromptAttribute, IVariablesContext variablesContext) : 
            base(parameterPromptAttribute, variablesContext)
        {
        }
    }
}
