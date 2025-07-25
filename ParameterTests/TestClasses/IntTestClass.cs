using ParameterModel.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterTests.TestClasses
{
    public class IntTestClass
    {
        /// <summary>
        /// Simple parameter but no variable option.
        /// </summary>
        [Parameter]
        public int Int1 { get; set; } = 1;

        /// <summary>
        /// Parameter with variable option.
        /// </summary>
        [Parameter(true)]
        public int Int2 { get; set; } = 2;

        /// <summary>
        /// Parameter with variable option.
        /// Cannot be edited by user but can be assigned in code.
        /// </summary>
        [Parameter(true)]
        [Editable(false)]
        public int Int3 { get; set; } = 3;

        /// <summary>
        /// Not a parameter.
        /// </summary>
        public int Int4 { get; set; } = 4;
        // Should pass.

        /// <summary>
        /// Simple parameter.
        /// Cannot be edited by user but can be assigned in code.
        /// </summary>
        [Parameter]
        [Editable(false)]
        public int Int5 { get; set; } = 5;
    }
}
