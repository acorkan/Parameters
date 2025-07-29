using ParameterModel.Attributes;
using ParameterModel.Variables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ParameterModel.Interfaces;
using ParameterTests.Tests;

namespace ParameterTests.TestClasses
{
    public class VariableParamTestClass : IntAndFloatParameterModelTests
    {
        /// <summary>
        /// Boolean varaible type.
        /// </summary>
        [VariableAssignment(VariableType.Boolean)]
        [Display(Description = "Select a boolean variable for Var1", Prompt = "Prompt for Var1")]
        public VariableProperty Var1 { get; set; } = new VariableProperty("BoolVar1");

        [Display(Description = "Select an integer variable for Var2", Prompt = "Prompt for Var2")]
        [VariableAssignment(VariableType.Integer)]
        public VariableProperty Var2 { get; set; } = new VariableProperty("IntVar1");

        [Display(Description = "Select a string variable for Var3", Prompt = "Prompt for Var3")]
        [VariableAssignment(VariableType.String, VariableAccessType.WriteOnly)]
        public VariableProperty Var3 { get; set; } = null;

        /// <summary>
        /// Not a parameter.
        /// </summary>
        public VariableProperty Var4 { get; set; }

        [Display(Description = "Select a numeric variable for Var5", Prompt = "Prompt for Var5")]
        [VariableAssignment([VariableType.Integer, VariableType.Float])]
        [Editable(false)]
        public VariableProperty Var5 { get; set; } = null;

        /// <summary>
        /// Not a parameter.
        /// </summary>
        [Display(Description = "Select a variable for Var6", Prompt = "Prompt for Var6")]
        public VariableProperty Var6 { get; set; } = new VariableProperty("FloatVar6");
    }
}
