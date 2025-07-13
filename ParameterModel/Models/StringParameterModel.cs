using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ParameterModel.Models
{
    public class StringParameterModel : ParameterModelBase
    {
        public StringParameterModel(ParameterAttribute parameterPromptAttribute, IImplementsParameterAttribute propertyOwner,
            IVariablesContext variablesContext)
            : base(parameterPromptAttribute, propertyOwner, variablesContext)
        {
        }
    }
}
