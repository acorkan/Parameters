using Newtonsoft.Json.Linq;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Documents;

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
            public string TestEnum1 { get; set; } = OptionEnum.Option2.ToString();

            [Parameter(Label = "Option", ToolTipNotes = "Select the option2 to use",
                EnumType = typeof(OptionEnum))]
            public string TestEnum2 { get; set; } = OptionEnum.Option5.ToString();

            [Parameter(Label = "Option", ToolTipNotes = "Select the option3 to use",
                EnumType = typeof(OptionEnum))]
            public string TestEnum3 { get; set; }

            [Parameter(ToolTipNotes = "This is a boolean parameter.")]
            public bool TestBool { get; set; } = true;

            [Parameter(AllowEmptyString =true, ToolTipNotes = "This is a string parameter.")]
            public string TestString1 { get; set; } = "";

            [Parameter(AllowEmptyString = false, ToolTipNotes = "This is a string parameter.")]
            public string TestString2{ get; set; } = null;

            [Parameter(AllowEmptyString = false, ToolTipNotes = "This is a string parameter.")]
            public string TestString3{ get; set; } = "ABcd";

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

            //[Parameter]
            //public OptionEnum OptionEnum1 { get; set; } = OptionEnum.Option3;

            //[Parameter]
            //public OptionEnum OptionEnum2 { get; set; } = OptionEnum.Option1;
        }

        private List<string> _testData = new List<string>
        {
            "TestInt3, System.Int32, 42, TestInt3, True, , ",
            "TestFloat3, System.Single, 3.14, TestFloat3, True, , ",
            "TestInt1, System.Int32, 42, TestInt1, True, , ",
            "TestInt2, System.Int32, 42, TestInt2, False, Value must be less than or equal to 40, ",
            "TestFloat1, System.Single, 3.14, TestFloat1, True, , ",
            "TestFloat2, System.Single, 3.14, TestFloat2, False, Value must be less than or equal to 2, ",
            "TestEnum1, System.String, Option2, Option, True, , Option 1 Description|Option 2 Description|Option 3 Description|Option4|Option5",
            "TestEnum2, System.String, Option5, Option, True, , Option 1 Description|Option 2 Description|Option 3 Description|Option4|Option5",
            "TestEnum3, System.String, , Option, False, Unable to resolve value of TestEnum3 type String, Option 1 Description|Option 2 Description|Option 3 Description|Option4|Option5",
            "TestBool, System.Boolean, True, TestBool, True, , True|False",
            "TestString1, System.String, , TestString1, True, , ",
            "TestString2, System.String, , TestString2, False, Entry cannot be blank, ",
            "TestString3, System.String, ABcd, TestString3, True, , ",
            "TestStringArray3, System.String[], Value1,Value2, TestStringArray3, True, , ",
            "TestStringArray1, System.String[], , TestStringArray1, True, , ",
            "TestStringArray2, System.String[], , TestStringArray2, True, , ",
        };

        [SetUp]
        public void Setup()
        {
        } 

        [Test]
        public void Test1()
        {
            ParameterClassLiteral test1 = new ParameterClassLiteral();
            string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            ParameterClassLiteral test2 = JsonSerializer.Deserialize<ParameterClassLiteral>(json);
        }

        [Test]
        public void Test2()
        {
            List<IParameterModel> models = ParameterModelHelper.Collect(new ParameterClassLiteral());
            Console.WriteLine($"Property name, Type, Format, Label, Validate, Errors, Selections");
            List<string> errors = new List<string>();
            List<string> output = new List<string>();
            foreach (IParameterModel model in models)
            {
                bool valid = model.Validate(errors);
                string outString = $"{model.PropertyInfo.Name}, {model.PropertyInfo.PropertyType}, {model.Format()}, {model.ParameterAttribute.Label}, {valid}, {string.Join('|', errors)}, {string.Join('|', model.GetSelections())}";
                output.Add(outString);
                Console.WriteLine(outString);
            }
            foreach(string testData in _testData)
            {
                Assert.IsTrue(output.Contains(testData), $"Output does not contain expected data: {testData}");
            }
            Assert.Pass();
        }
    }
}