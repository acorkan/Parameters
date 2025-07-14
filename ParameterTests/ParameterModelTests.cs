using log4net;
using ParameterModel.Attributes;
using ParameterModel.Factories;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using ParameterModel.Variables;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.Foundation.Metadata;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace ParameterTests
{
    public class ParameterModelTests
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



        //internal class MockExecutionContext : EvaluationContextBase
        //{
        //    IVariablesContext _variablesContext = new VariablesContext(new VariableFactory(), new VariableCollectionFactory(), new VariableValueFactory());
        //    public MockExecutionContext() : base(_variablesContext, false, false)
        //    {
        //    }
        //    public MockExecutionContext(ILog log, bool isDebug, bool isSimulation) : base(log, isDebug, isSimulation)
        //    {
        //    }
        //}

        //internal class ParameterClassSimple : IImplementsParameterAttribute
        //{
        //    [Parameter(Label = "Option", ToolTipNotes = "Select the option to use",
        //        EnumType = typeof(OptionEnum))]
        //    public string TestEnum1 { get; set; } = OptionEnum.Option2.ToString();

        //    [Parameter(Label = "Option", ToolTipNotes = "Select the option2 to use",
        //        EnumType = typeof(OptionEnum))]
        //    public string TestEnum2 { get; set; } = OptionEnum.Option5.ToString();

        //    [Parameter(Label = "Option", ToolTipNotes = "Select the option3 to use",
        //        EnumType = typeof(OptionEnum))]
        //    public string TestEnum3 { get; set; }

        //    [Parameter(ToolTipNotes = "This is a boolean parameter.")]
        //    public bool TestBool { get; set; } = true;

        //    [Parameter(AllowEmptyString = true, ToolTipNotes = "This is a string parameter.")]
        //    public string TestString1 { get; set; } = "";

        //    [Parameter(AllowEmptyString = false, ToolTipNotes = "This is a string parameter.")]
        //    public string TestString2 { get; set; } = null;

        //    [Parameter(AllowEmptyString = false, ToolTipNotes = "This is a string parameter.")]
        //    public string TestString3 { get; set; } = "ABcd";

        //    [Parameter(PresentationOrder = 1)]
        //    public int TestInt1 { get; set; } = 42;

        //    [Parameter(PresentationOrder = 1, Max = 40)]
        //    public int TestInt2 { get; set; } = 42;

        //    [Parameter(PresentationOrder = 0, Max = 50, Min = 0)]
        //    public int TestInt3 { get; set; } = 42;

        //    [Parameter(PresentationOrder = 1)]
        //    public float TestFloat1 { get; set; } = 3.14f;

        //    [Parameter(PresentationOrder = 1, Min = -3, Max = 2)]
        //    public float TestFloat2 { get; set; } = 3.14f;

        //    [Parameter(PresentationOrder = 0, Min = 0, Max = 5)]
        //    public float TestFloat3 { get; set; } = 3.14f;

        //    [Parameter(PresentationOrder = 11)]
        //    public string[] TestStringArray1 { get; set; } = [];

        //    [Parameter(PresentationOrder = 11, AllowEmptyString = true)]
        //    public string[] TestStringArray2 { get; set; } = [];

        //    [Parameter(PresentationOrder = 10, AllowEmptyString = false)]
        //    public string[] TestStringArray3 { get; set; } = { "Value1", "Value2" };

        //    [Parameter]
        //    public OptionEnum OptionEnum1 { get; set; } = OptionEnum.Option3;

        //    [Parameter]
        //    public OptionEnum OptionEnum2 { get; set; } = OptionEnum.Option1;
        //}

        //internal class ParameterClassLiteral : ParameterClassSimple
        //{
        //    [Parameter(PresentationOrder = 1, Max = 40)]
        //    public string TestVarInt { get; set; } = "42";

        //    [Parameter(PresentationOrder = 0)]
        //    [Range(0, 50)]
        //    public string TestVarBool { get; set; } = "False";

        //    [Parameter(PresentationOrder = 1)]
        //    public string TestVarFloat { get; set; } = "3.14";

        //    [Parameter(VariableType =typeof(string[]))]
        //    public string TestVarStringArray1 { get; set; } = "Value1 Value2";
        //}

        //private List<string> _testData = new List<string>
        //{
        //    "TestInt3, System.Int32, 42, TestInt3, True, , ",
        //    "TestFloat3, System.Single, 3.14, TestFloat3, True, , ",
        //    "TestInt1, System.Int32, 42, TestInt1, True, , ",
        //    "TestInt2, System.Int32, 42, TestInt2, False, Value must be less than or equal to 40, ",
        //    "TestFloat1, System.Single, 3.14, TestFloat1, True, , ",
        //    "TestFloat2, System.Single, 3.14, TestFloat2, False, Value must be less than or equal to 2, ",
        //    "TestEnum1, System.String, Option2, Option, True, , Option 1 Description|Option 2 Description|Option 3 Description|Option4|Option5",
        //    "TestEnum2, System.String, Option5, Option, True, , Option 1 Description|Option 2 Description|Option 3 Description|Option4|Option5",
        //    "TestEnum3, System.String, , Option, False, Unable to resolve value of TestEnum3 type String, Option 1 Description|Option 2 Description|Option 3 Description|Option4|Option5",
        //    "TestBool, System.Boolean, True, TestBool, True, , True|False",
        //    "TestString1, System.String, , TestString1, True, , ",
        //    "TestString2, System.String, , TestString2, False, Entry cannot be blank, ",
        //    "TestString3, System.String, ABcd, TestString3, True, , ",
        //    "TestStringArray3, System.String[], Value1,Value2, TestStringArray3, True, , ",
        //    "TestStringArray1, System.String[], , TestStringArray1, True, , ",
        //    "TestStringArray2, System.String[], , TestStringArray2, True, , ",
        //};

        //[SetUp]
        //public void Setup()
        //{
        //}

        ////[Test]
        ////public void Test1()
        ////{
        ////    ParameterClassLiteral test1 = new ParameterClassLiteral();
        ////    string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
        ////    {
        ////        WriteIndented = true,
        ////        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        ////    });
        ////    ParameterClassLiteral test2 = JsonSerializer.Deserialize<ParameterClassLiteral>(json);
        ////}

        ////[Test]
        ////public void Test2()
        ////{
        ////    List<IParameterModel> models = _parameterModelHelper.Collect(new ParameterClassLiteral());
        ////    Console.WriteLine($"Property name, Type, Format, Label, Validate, Errors, Selections");
        ////    List<string> errors = new List<string>();
        ////    List<string> output = new List<string>();
        ////    foreach (IParameterModel model in models)
        ////    {
        ////        bool valid = model.IsValid(errors);
        ////        string outString = $"{model.PropertyInfo.Name}, {model.PropertyInfo.PropertyType}, {model.ToDisplayString()}, {model.ParameterAttribute.Label}, {valid}, {string.Join('|', errors)}, {string.Join('|', model.GetSelectionItems())}";
        ////        output.Add(outString);
        ////        Console.WriteLine(outString);
        ////    }
        ////    foreach(string testData in _testData)
        ////    {
        ////        Assert.IsTrue(output.Contains(testData), $"Output does not contain expected data: {testData}");
        ////    }
        ////    Assert.Pass();
        ////}

        //#region Boolean tests
        internal class BoolTests : IImplementsParameterAttribute
        {
            // Should pass.
            [Parameter]
            public bool TestBool1 { get; set; } = true;
            // Should pass.
            [Parameter]
            public bool TestBool2 { get; set; } = false;
            // Should pass.
            [Parameter]
            public bool TestBool3 { get; set; }
            // Should NOT pass for garbage string.
            [Parameter]
            public bool TestBool4 { get; set; }
            // Should NOT pass for multiple items.
            [Parameter]
            public bool TestBool5 { get; set; }
            // Should pass if variables are supplied.
            [Parameter]
            public bool TestBool6 { get; set; }

            public Dictionary<string, string> VariableAssignments { get; } = new Dictionary<string, string>();

            public Dictionary<string, ParameterAttribute> AttributeMap { get; } = new Dictionary<string, ParameterAttribute>();
        }

        [Test]
        public void TestBoolSerialize1()
        {
            //Dictionary<string, IParameterModel> promptModels = _parameterModelHelper.Collect(new BoolTests());

            BoolTests test1 = new BoolTests();
            string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            BoolTests test2 = JsonSerializer.Deserialize<BoolTests>(json);
            Assert.AreEqual(test1.TestBool1, test2.TestBool1, "TestBool1 should be equal after serialization and deserialization.");
            Assert.AreEqual(test1.TestBool2, test2.TestBool2, "TestBool2 should be equal after serialization and deserialization.");
            Assert.AreEqual(test1.TestBool3, test2.TestBool3, "TestBool3 should be equal after serialization and deserialization.");
            Assert.AreEqual(test1.TestBool4, test2.TestBool4, "TestBool4 should be equal after serialization and deserialization.");
            Assert.AreEqual(test1.TestBool5, test2.TestBool5, "TestBool5 should be equal after serialization and deserialization.");
        }

        [Test]
        public void TestBoolSerialize2()
        {
            BoolTests test1 = new BoolTests();
            string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            BoolTests test2 = JsonSerializer.Deserialize<BoolTests>(json);

            //MockExecutionContext executionContext = new MockExecutionContext();
            IVariablesContext variablesContext = new VariablesContext();
            ParameterModelFactory parameterModelHelper = new ParameterModelFactory(variablesContext);

            Dictionary<string, IParameterModel> testModels1 = parameterModelHelper.CollectModels(test1);
            Dictionary<string, IParameterModel> testModels2 = parameterModelHelper.CollectModels(test1);
            foreach (KeyValuePair<string, IParameterModel> model in testModels1)
            {
                string paramName = model.Key;
                IParameterModel model2 = testModels2[paramName];
                Assert.AreEqual(model.Value.GetDisplayString(), model2.GetDisplayString(), $"Property display string mismatch for {paramName}");
            }
        }

        //[Test]
        //public void TestBoolErrors()
        //{
        //    MockExecutionContext executionContext = new MockExecutionContext();
        //    executionContext.ClearVariables();
        //    VariableBase vb = executionContext.AddVariable("boolVar", VariableType.Boolean, VariableSource.UserDefined);
        //    vb.SetValue(true);
        //    ParameterModelHelper parameterModelHelper = new ParameterModelHelper(executionContext, log);

        //    BoolTests test1 = new BoolTests();
        //    Dictionary<string, IParameterModel> testModels1 = parameterModelHelper.Collect(test1);
        //    Assert.IsFalse(testModels1["TestBool1"].IsAttributeError, "TestBool1 should never have an attribute error.");
        //    Assert.IsFalse(testModels1["TestBool1"].IsVariableError, "TestBool1 should resolver to type.");

        //    Assert.IsFalse(testModels1["TestBool2"].IsAttributeError, "TestBool2 should never have an attribute error.");
        //    Assert.IsFalse(testModels1["TestBool2"].IsVariableError, "TestBool2 should resolve to type.");

        //    Assert.IsFalse(testModels1["TestBool3"].IsAttributeError, "TestBool3 should never have an attribute error.");
        //    Assert.IsFalse(testModels1["TestBool3"].IsVariableError, "TestBool3 should resolve to a valid seting.");

        //    Assert.IsFalse(testModels1["TestBool4"].IsAttributeError, "TestBool4 should never have an attribute error.");
        //    Assert.IsTrue(testModels1["TestBool4"].IsVariableError, "TestBool4 should not resolve to anything valid.");

        //    Assert.IsFalse(testModels1["TestBool5"].IsAttributeError, "TestBool5 should never have an attribute error.");
        //    Assert.IsTrue(testModels1["TestBool5"].IsVariableError, "TestBool5 should not resolve to anything valid.");

        //    Assert.IsFalse(testModels1["TestBool6"].IsAttributeError, "TestBool6 should never have an attribute error.");
        //    Assert.IsFalse(testModels1["TestBool6"].IsVariableError, "TestBool6 should resolve to a variable.");

        //    string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //    });
        //    BoolTests test2 = JsonSerializer.Deserialize<BoolTests>(json);
        //    executionContext.ClearVariables();
        //    Dictionary<string, IParameterModel> testModels2 = parameterModelHelper.Collect(test1);

        //    Assert.IsFalse(testModels2["TestBool1"].IsAttributeError, "TestBool1 should never have an attribute error.");
        //    Assert.IsFalse(testModels2["TestBool1"].IsVariableError, "TestBool1 should resolver to type.");

        //    Assert.IsFalse(testModels2["TestBool2"].IsAttributeError, "TestBool2 should never have an attribute error.");
        //    Assert.IsFalse(testModels2["TestBool2"].IsVariableError, "TestBool2 should resolve to type.");

        //    Assert.IsFalse(testModels2["TestBool3"].IsAttributeError, "TestBool3 should never have an attribute error.");
        //    Assert.IsFalse(testModels2["TestBool3"].IsVariableError, "TestBool3 should resolve to a valid seting.");

        //    Assert.IsFalse(testModels2["TestBool4"].IsAttributeError, "TestBool4 should never have an attribute error.");
        //    Assert.IsTrue(testModels2["TestBool4"].IsVariableError, "TestBool4 should not resolve to anything valid.");

        //    Assert.IsFalse(testModels2["TestBool5"].IsAttributeError, "TestBool5 should never have an attribute error.");
        //    Assert.IsTrue(testModels2["TestBool5"].IsVariableError, "TestBool5 should not resolve to anything valid.");

        //    Assert.IsFalse(testModels2["TestBool6"].IsAttributeError, "TestBool6 should never have an attribute error.");
        //    Assert.IsTrue(testModels2["TestBool6"].IsVariableError, "TestBool6 should not resolve to anything valid because the variable should be gone.");

        //    //string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
        //    //{
        //    //    WriteIndented = true,
        //    //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //    //});
        //    //BoolTests test2 = JsonSerializer.Deserialize<BoolTests>(json);

        //    //Dictionary<string, IParameterModel> testModels2 = _parameterModelHelper.Collect(test1);
        //    //foreach (KeyValuePair<string, IParameterModel> model in testModels1)
        //    //{
        //    //    string paramName = model.Key;
        //    //    IParameterModel model2 = testModels2[paramName];
        //    //    Assert.AreEqual(model.Value.ToDisplayString(), model2.ToDisplayString(), $"Property display string mismatch for {paramName}");
        //    //}
        //}

        //[Test]
        //public void TestBoolSerialize3()
        //{
        //    BoolTests test1 = new BoolTests();
        //    string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //    });
        //    BoolTests test2 = JsonSerializer.Deserialize<BoolTests>(json);

        //    MockExecutionContext executionContext = new MockExecutionContext();
        //    ParameterModelHelper parameterModelHelper = new ParameterModelHelper(executionContext, log);

        //    Dictionary<string, IParameterModel> testModels1 = parameterModelHelper.Collect(test1);
        //    Dictionary<string, IParameterModel> testModels2 = parameterModelHelper.Collect(test1);
        //    foreach (KeyValuePair<string, IParameterModel> model in testModels1)
        //    {
        //        string paramName = model.Key;
        //        IParameterModel model2 = testModels2[paramName];
        //        Assert.AreEqual(model.Value.ToDisplayString(), model2.ToDisplayString(), $"Property display string mismatch for {paramName}");
        //    }
        //}

        //#endregion Boolean tests
        //#region Int tests
        //internal class IntTests : IImplementsParameterAttribute
        //{
        //    // Should pass.
        //    [Parameter]
        //    public int Testint1 { get; set; } = 10;
        //    // Should pass.
        //    [Parameter]
        //    public int Testint2 { get; set; } = -20;
        //    // Should pass.
        //    [Parameter(VariableType = typeof(int))]
        //    public string Testint3 { get; set; } = "33";
        //    // Should NOT pass for garbage string.
        //    [Parameter(VariableType = typeof(int))]
        //    public string Testint4 { get; set; } = "lfsdl";
        //    // Should NOT pass for multiple items.
        //    [Parameter(VariableType = typeof(int))]
        //    public string Testint5 { get; set; } = "intVar foo";
        //    // Should pass if variables are supplied.
        //    [Parameter(VariableType = typeof(int))]
        //    public string Testint6 { get; set; } = "intVar";
        //    // Should pass.
        //    [Parameter]
        //    [Range(0, 100)]
        //    public int Testint7 { get; set; } = 77;
        //    // Should pass.
        //    [Parameter]
        //    public int Testint8 { get; set; } = -88;
        //}

        //[Test]
        //public void TestIntSerialize1()
        //{
        //    //Dictionary<string, IParameterModel> promptModels = _parameterModelHelper.Collect(new BoolTests());

        //    BoolTests test1 = new BoolTests();
        //    string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //    });
        //    BoolTests test2 = JsonSerializer.Deserialize<BoolTests>(json);
        //    Assert.AreEqual(test1.TestBool1, test2.TestBool1, "TestBool1 should be equal after serialization and deserialization.");
        //    Assert.AreEqual(test1.TestBool2, test2.TestBool2, "TestBool2 should be equal after serialization and deserialization.");
        //    Assert.AreEqual(test1.TestBool3, test2.TestBool3, "TestBool3 should be equal after serialization and deserialization.");
        //    Assert.AreEqual(test1.TestBool4, test2.TestBool4, "TestBool4 should be equal after serialization and deserialization.");
        //    Assert.AreEqual(test1.TestBool5, test2.TestBool5, "TestBool5 should be equal after serialization and deserialization.");
        //}

        //[Test]
        //public void TestIntSerialize2()
        //{
        //    BoolTests test1 = new BoolTests();
        //    string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //    });
        //    BoolTests test2 = JsonSerializer.Deserialize<BoolTests>(json);

        //    MockExecutionContext executionContext = new MockExecutionContext();
        //    ParameterModelHelper parameterModelHelper = new ParameterModelHelper(executionContext, log);

        //    Dictionary<string, IParameterModel> testModels1 = parameterModelHelper.Collect(test1);
        //    Dictionary<string, IParameterModel> testModels2 = parameterModelHelper.Collect(test1);
        //    foreach (KeyValuePair<string, IParameterModel> model in testModels1)
        //    {
        //        string paramName = model.Key;
        //        IParameterModel model2 = testModels2[paramName];
        //        Assert.AreEqual(model.Value.ToDisplayString(), model2.ToDisplayString(), $"Property display string mismatch for {paramName}");
        //    }
        //}

        //[Test]
        //public void TestIntErrors()
        //{
        //    MockExecutionContext executionContext = new MockExecutionContext();
        //    executionContext.ClearVariables();
        //    VariableBase vb = executionContext.AddVariable("boolVar", VariableType.Boolean, VariableSource.UserDefined);
        //    vb.SetValue(true);
        //    ParameterModelHelper parameterModelHelper = new ParameterModelHelper(executionContext, log);

        //    BoolTests test1 = new BoolTests();
        //    Dictionary<string, IParameterModel> testModels1 = parameterModelHelper.Collect(test1);
        //    Assert.IsFalse(testModels1["TestBool1"].IsAttributeError, "TestBool1 should never have an attribute error.");
        //    Assert.IsFalse(testModels1["TestBool1"].IsVariableError, "TestBool1 should resolver to type.");

        //    Assert.IsFalse(testModels1["TestBool2"].IsAttributeError, "TestBool2 should never have an attribute error.");
        //    Assert.IsFalse(testModels1["TestBool2"].IsVariableError, "TestBool2 should resolve to type.");

        //    Assert.IsFalse(testModels1["TestBool3"].IsAttributeError, "TestBool3 should never have an attribute error.");
        //    Assert.IsFalse(testModels1["TestBool3"].IsVariableError, "TestBool3 should resolve to a valid seting.");

        //    Assert.IsFalse(testModels1["TestBool4"].IsAttributeError, "TestBool4 should never have an attribute error.");
        //    Assert.IsTrue(testModels1["TestBool4"].IsVariableError, "TestBool4 should not resolve to anything valid.");

        //    Assert.IsFalse(testModels1["TestBool5"].IsAttributeError, "TestBool5 should never have an attribute error.");
        //    Assert.IsTrue(testModels1["TestBool5"].IsVariableError, "TestBool5 should not resolve to anything valid.");

        //    Assert.IsFalse(testModels1["TestBool6"].IsAttributeError, "TestBool6 should never have an attribute error.");
        //    Assert.IsFalse(testModels1["TestBool6"].IsVariableError, "TestBool6 should resolve to a variable.");

        //    string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //    });
        //    BoolTests test2 = JsonSerializer.Deserialize<BoolTests>(json);
        //    executionContext.ClearVariables();
        //    Dictionary<string, IParameterModel> testModels2 = parameterModelHelper.Collect(test1);

        //    Assert.IsFalse(testModels2["TestBool1"].IsAttributeError, "TestBool1 should never have an attribute error.");
        //    Assert.IsFalse(testModels2["TestBool1"].IsVariableError, "TestBool1 should resolver to type.");

        //    Assert.IsFalse(testModels2["TestBool2"].IsAttributeError, "TestBool2 should never have an attribute error.");
        //    Assert.IsFalse(testModels2["TestBool2"].IsVariableError, "TestBool2 should resolve to type.");

        //    Assert.IsFalse(testModels2["TestBool3"].IsAttributeError, "TestBool3 should never have an attribute error.");
        //    Assert.IsFalse(testModels2["TestBool3"].IsVariableError, "TestBool3 should resolve to a valid seting.");

        //    Assert.IsFalse(testModels2["TestBool4"].IsAttributeError, "TestBool4 should never have an attribute error.");
        //    Assert.IsTrue(testModels2["TestBool4"].IsVariableError, "TestBool4 should not resolve to anything valid.");

        //    Assert.IsFalse(testModels2["TestBool5"].IsAttributeError, "TestBool5 should never have an attribute error.");
        //    Assert.IsTrue(testModels2["TestBool5"].IsVariableError, "TestBool5 should not resolve to anything valid.");

        //    Assert.IsFalse(testModels2["TestBool6"].IsAttributeError, "TestBool6 should never have an attribute error.");
        //    Assert.IsTrue(testModels2["TestBool6"].IsVariableError, "TestBool6 should not resolve to anything valid because the variable should be gone.");

        //    //string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
        //    //{
        //    //    WriteIndented = true,
        //    //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //    //});
        //    //BoolTests test2 = JsonSerializer.Deserialize<BoolTests>(json);

        //    //Dictionary<string, IParameterModel> testModels2 = _parameterModelHelper.Collect(test1);
        //    //foreach (KeyValuePair<string, IParameterModel> model in testModels1)
        //    //{
        //    //    string paramName = model.Key;
        //    //    IParameterModel model2 = testModels2[paramName];
        //    //    Assert.AreEqual(model.Value.ToDisplayString(), model2.ToDisplayString(), $"Property display string mismatch for {paramName}");
        //    //}
        //}

        //[Test]
        //public void TestBoolSerialize3()
        //{
        //    BoolTests test1 = new BoolTests();
        //    string json = JsonSerializer.Serialize(test1, new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //    });
        //    BoolTests test2 = JsonSerializer.Deserialize<BoolTests>(json);

        //    MockExecutionContext executionContext = new MockExecutionContext();
        //    ParameterModelHelper parameterModelHelper = new ParameterModelHelper(executionContext, log);

        //    Dictionary<string, IParameterModel> testModels1 = parameterModelHelper.Collect(test1);
        //    Dictionary<string, IParameterModel> testModels2 = parameterModelHelper.Collect(test1);
        //    foreach (KeyValuePair<string, IParameterModel> model in testModels1)
        //    {
        //        string paramName = model.Key;
        //        IParameterModel model2 = testModels2[paramName];
        //        Assert.AreEqual(model.Value.ToDisplayString(), model2.ToDisplayString(), $"Property display string mismatch for {paramName}");
        //    }
        //}

        //#endregion Int tests
    }
}