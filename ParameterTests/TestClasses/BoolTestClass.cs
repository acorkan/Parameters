using ParameterModel.Attributes;
using ParameterModel.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ParameterTests.TestClasses
{
    public class BoolTestClass : ImplementsParametersBase
    {
        /// <summary>
        /// Simple parameter but no variable option.
        /// </summary>
        [Parameter]
        public bool Bool1 { get; set; } = true;

        /// <summary>
        /// Parameter with variable option.
        /// </summary>
        [Parameter(true)]
        public bool Bool2 { get; set; } = false;

        /// <summary>
        /// Parameter with variable option.
        /// Cannot be edited by user but can be assigned in code.
        /// </summary>
        [Parameter(true)]
        [Editable(false)]
        public bool Bool3 { get; set; } = true;

        /// <summary>
        /// Not a parameter.
        /// </summary>
        public bool Bool4 { get; set; } = true;

        /// <summary>
        /// Simple parameter.
        /// Cannot be edited by user but can be assigned in code.
        /// </summary>
        [Parameter]
        [Editable(false)]
        public bool Bool5 { get; set; } = true;
    }

    public class BoolTestBadAttribute : BoolTestClass
    {
        // Should pass.
        [Parameter]
        [System.ComponentModel.DataAnnotations.Range(2, 4)]
        [StringLength(3)]
        public bool HasRange { get; set; }
        // Should pass.
        [Parameter]
        [System.ComponentModel.DataAnnotations.Range(4, 2)]
        public bool HasReversedRange { get; set; }
    }
}
