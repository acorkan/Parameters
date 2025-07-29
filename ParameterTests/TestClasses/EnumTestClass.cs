using ParameterModel.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterTests.TestClasses
{
    public enum OptionEnum
    {
        [System.ComponentModel.Description("Option 1 Description")]
        Option1 = 0,
        [System.ComponentModel.Description("Option 2 Description")]
        Option2 = 1,
        [System.ComponentModel.Description("Option 3 Description")]
        Option3 = 3,
        Option4 = 4,
        Option5 = 5
    }

    public enum Operations
    {
        [System.ComponentModel.Description("Aspirate liquid")]
        Aspirate = 0,
        [System.ComponentModel.Description("Single aspirate / Multi dispense")]
        SAMD = 2,
        [System.ComponentModel.Description("Mix")]
        Mix = 3,
        Load = 4,
        Eject = 5
    }

    public class EnumTestClass : VariableParamTestClass
    {
        [Parameter(true)]
        [Display(Description = "Select an OptionEnum or variable", Prompt = "Prompt for Enum1")]
        public OptionEnum Enum1 { get; set; }

        [Parameter]
        [Display(Description = "Select an OptionEnum ", Prompt = "Prompt for Enum2")]
        public OptionEnum Enum2 { get; set; }

        [Parameter]
        [Editable(false)]
        [Display(Description = "Select an Operations", Prompt = "Prompt for Int1")]
        public Operations FirstOperation { get; set; } = Operations.SAMD;

        [Parameter(true)]
        [Editable(false)]
        [Display(Description = "Select an Operations", Prompt = "Prompt for Int1")]
        public Operations SecondOperation { get; set; } = Operations.Load;
    }
}
