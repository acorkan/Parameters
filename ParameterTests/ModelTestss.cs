using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models;

namespace ParameterTests
{
    public class Tests
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

        internal class ParameterClassLiteral : IImplementsParameterAttribute
        {
            [Parameter(Label ="Option", ToolTipNotes ="Select the option to use", 
                EnumType =typeof(OptionEnum))]
            public int TestEnum1 { get; set; } = (int)OptionEnum.Option1;

            [Parameter(Label = "Option", ToolTipNotes = "Select the option to use",
                EnumType = typeof(OptionEnum))]
            public int TestEnum2 { get; set; } = 0;

            [Parameter(ToolTipNotes = "This is a boolean parameter.")]
            public bool TestBool { get; set; } = true;

            [Parameter(AllowEmptyString =true, ToolTipNotes = "This is a string parameter.")]
            public string TestString1 { get; set; } = "";

            [Parameter(AllowEmptyString = false, ToolTipNotes = "This is a string parameter.")]
            public string TestString2{ get; set; } = null;

            [Parameter(AllowEmptyString = false, ToolTipNotes = "This is a string parameter.")]
            public string TestString4{ get; set; } = "ABcd";

            [Parameter(PresentationOrder = 1)]
            public int TestInt1 { get; set; } = 42;

            [Parameter(PresentationOrder = 1, Max = 40)]
            public int TestInt2 { get; set; } = 42;

            [Parameter(PresentationOrder = 0, Max = 50, Min = 0)]
            public int TestInt3 { get; set; } = 42;

            [Parameter(PresentationOrder = 1)]
            public float TestFloat1 { get; set; } = 3.14f;

            [Parameter(PresentationOrder = 1, Min =-3, Max =2)]
            public float TestFloat2 { get; set; } = 3.14f;

            [Parameter(PresentationOrder = 0, Min = 0, Max = 5)]
            public float TestFloat3 { get; set; } = 3.14f;

            [Parameter(PresentationOrder = 11)]
            public string[] TestStringArray1 { get; set; } = [];

            [Parameter(PresentationOrder = 11, AllowEmptyString =true)]
            public string[] TestStringArray2 { get; set; } = [];

            [Parameter(PresentationOrder = 10, AllowEmptyString = false)]
            public string[] TestStringArray3 { get; set; } = { "Value1", "Value2" };
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            List<IParameterModel> models = ParameterModelHelper.Collect(new ParameterClassLiteral());
            Console.WriteLine($"Property name, Type, Format, Label, Validate, Errors, Selections");
            List<string> errors = new List<string>();
            foreach (IParameterModel model in models)
            {
                Console.WriteLine($"{model.PropertyInfo.Name}, {model.PropertyInfo.PropertyType}, {model.Format()}, {model.ParameterAttribute.Label}, {model.Validate(errors)}, {string.Join('|', errors)}, {string.Join('|', model.GetSelections())}");
            }
            Assert.Pass();
        }
    }
}