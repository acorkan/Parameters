using ParameterModel.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterTests.TestClasses
{
    public class GeneralTestClass : BoolTestClass
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

        /// <summary>
        /// Simple parameter but no variable option.
        /// </summary>
        [Parameter]
        [Display(Description = "Just enter a number, no variable allowed", Prompt = "Prompt for Float1")]
        public float Float1 { get; set; } = 1.1F;

        /// <summary>
        /// Parameter with variable option.
        /// </summary>
        [Parameter(true)]
        [Display(Description = "Enter a number or variable", Prompt = "Prompt for Float2")]
        public float Float2 { get; set; } = 2.2F;

        /// <summary>
        /// Parameter with variable option.
        /// Cannot be edited by user but can be assigned in code.
        /// </summary>
        [Parameter(true)]
        [Editable(false)]
        [Display(Description = "Should not be editable", Prompt = "Prompt for Float3")]
        public float Float3 { get; set; } = 3.3F;

        /// <summary>
        /// Not a parameter.
        /// </summary>
        public float Float4 { get; set; } = 4.4F;
        // Should pass.

        /// <summary>
        /// Simple parameter.
        /// Cannot be edited by user but can be assigned in code.
        /// </summary>
        [Parameter]
        [Editable(true)]
        [Display(Description = "Enter a number in range or variable", Prompt = "Prompt for Float5")]
        [System.ComponentModel.DataAnnotations.Range(0.0, 5.2)]
        public float Float5 { get; set; } = 5.5F;

        /// <summary>
        /// Simple parameter but no variable option.
        /// Range 2 - 4.
        /// </summary>
        [Parameter]
        [System.ComponentModel.DataAnnotations.Range(2, 4)]
        [Display(Description = "Select an integer between 2 and 4", Prompt = "Prompt for Int1")]
        public int Int1 { get; set; } = 1;

        /// <summary>
        /// Parameter with variable option.
        /// Range 1 - 8.
        /// </summary>
        [Parameter(true)]
        [System.ComponentModel.DataAnnotations.Range(1, 8)]
        [Display(Description = "Select an integer between 2 and 4 or a variable", Prompt = "Prompt for Int2")]
        public int Int2 { get; set; } = 2;

        /// <summary>
        /// Parameter with variable option.
        /// Cannot be edited by user but can be assigned in code.
        /// Range 0 - 50.
        /// </summary>
        [Parameter(true)]
        [Editable(false)]
        [System.ComponentModel.DataAnnotations.Range(0, 50)]
        public int Int3 { get; set; } = 3;

        /// <summary>
        /// Not a parameter.
        /// Range 1 - 8.
        /// </summary>
        [System.ComponentModel.DataAnnotations.Range(1, 8)]
        public int Int4 { get; set; } = 4;
        // Should pass.

        /// <summary>
        /// Simple parameter.
        /// Cannot be edited by user but can be assigned in code.
        /// No Range.
        /// </summary>
        [Parameter]
        [Editable(false)]
        public int Int5 { get; set; } = 5;

    }
}
