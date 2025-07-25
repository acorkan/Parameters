using ParameterModel.Attributes;
using ParameterModel.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterTests.TestClasses
{
    public class IntTestClass : ImplementsParametersBase
    {
        /// <summary>
        /// Simple parameter but no variable option.
        /// Range 2 - 4.
        /// </summary>
        [Parameter]
        [System.ComponentModel.DataAnnotations.Range(2, 4)]
        public int Int1 { get; set; } = 1;

        /// <summary>
        /// Parameter with variable option.
        /// Range 1 - 8.
        /// </summary>
        [Parameter(true)]
        [System.ComponentModel.DataAnnotations.Range(1, 8)]
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
