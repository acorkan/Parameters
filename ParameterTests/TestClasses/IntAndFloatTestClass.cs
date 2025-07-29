using ParameterModel.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterTests.TestClasses
{
    public class IntAndFloatTestClass : IntTestClass
    {
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
    }
}
