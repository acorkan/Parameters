using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterTests.TestClasses
{
    internal enum OptionEnum
    {
        [System.ComponentModel.Description("Option 1 Description")]
        Option1 = 1,
        [System.ComponentModel.Description("Option 2 Description")]
        Option2 = 2,
        [System.ComponentModel.Description("Option 3 Description")]
        Option3 = 3,
        Option4 = 4,
        Option5 = 5
    }

    internal class EnumTestClass
    {
    }
}
