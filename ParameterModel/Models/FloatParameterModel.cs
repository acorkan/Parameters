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
    public class FloatParameterModel : ParameterModelBase
    {
        public FloatParameterModel(ParameterAttribute parameterPromptAttribute, IImplementsParameterAttribute propertyOwner,
                IVariablesContext variablesContext) : 
            base(parameterPromptAttribute, propertyOwner, variablesContext)
        {
        }

    }
}
