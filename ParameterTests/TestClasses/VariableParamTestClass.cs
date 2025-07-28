using ParameterModel.Attributes;
using ParameterModel.Variables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ParameterModel.Interfaces;

namespace ParameterTests.TestClasses
{
    public class VariableParamTestClass : BoolTestClass
    {
        /// <summary>
        /// Boolean varaible type.
        /// </summary>
        [VariableAssignment(VariableType.Boolean)]
        public VariableProperty Var1 { get; set; } = new VariableProperty("BoolVar1");

        [VariableAssignment(VariableType.Integer)]
        public VariableProperty Var2 { get; set; } = new VariableProperty("IntVar1");

        [Display(Name = "Variable 1", Description = "Description  for var1", Prompt = "Prompt for var1")]
        [VariableAssignment(VariableType.String, VariableAccessType.ReadOnly)]
        public VariableProperty Var3 { get; set; } = null;

        /// <summary>
        /// Not a parameter.
        /// </summary>
        public VariableProperty Var4 { get; set; }

        [VariableAssignment([VariableType.Integer, VariableType.Float])]
        [Editable(false)]
        public VariableProperty Var5 { get; set; } = null;

        /// <summary>
        /// Not a parameter.
        /// </summary>
        public VariableProperty Var6 { get; set; } = new VariableProperty("FloatVar6");
    }
}
