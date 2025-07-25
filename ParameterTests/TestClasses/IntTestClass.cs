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

        // Not a parameter.
        public int Int4 { get; set; } = 4;
        // Should pass.

        [Parameter]
        [Editable(false)]
        public int Int5 { get; set; } = 5;

        //// Should pass.
        //[Parameter]
        //public float TestFloat1 { get; set; } = 1;

        //// Should pass.
        //[Parameter(true)]
        //public float TestFloat2 { get; set; } = 2;

        //// Should pass.
        //[Parameter(true)]
        //[Editable(false)]
        //public float TestFloat3 { get; set; } = 3;

        //// Not a parameter.
        //public float TestFloat4 { get; set; } = 4;
        //// Should pass.

        //[Parameter]
        //[Editable(false)]
        //[Range(0.0, 10.0)]
        //public float TestFloat5 { get; set; } = 5;
    }
}
