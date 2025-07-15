using log4net;
using ParameterModel.Attributes;
using ParameterModel.Extensions;
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

        internal class ImplementsParametersBase : IImplementsParameterAttribute
        {
            public Dictionary<string, string> VariableAssignments { get; } = new Dictionary<string, string>();
            public Dictionary<string, ParameterAttribute> AttributeMap { get; } = new Dictionary<string, ParameterAttribute>();
        }

        #region Boolean tests
        internal class BoolTestClass : ImplementsParametersBase
        {
            // Should pass.
            [Parameter]
            public bool TestBool1 { get; set; } = true;

            // Should pass.
            [Parameter(CanBeVariable = true)]
            public bool TestBool2 { get; set; } = false;

            // Should pass.
            [Parameter(CanBeVariable = true)]
            [Editable(false)]
            public bool TestBool3 { get; set; }

            // Not a parameter.
            public bool TestBool4 { get; set; } = true;
            // Should pass.

            [Parameter]
            [Editable(false)]
            public bool TestBool5 { get; set; } = false;
            // Should pass.

            [Parameter(CanBeVariable = true)]
            public bool TestBool6 { get; set; } = true;
        }


        internal class BoolTestBadAttribute : BoolTestClass
        {
            // Should pass.
            [Parameter]
            [Range(2, 4)]
            public bool HasRange { get; set; }
            // Should pass.
            [Parameter]
            [Range(4, 2)]
            public bool HasReversedRange { get; set; }
        }


        [Test]
        public void TestBoolSerialize()
        {
            BoolTestClass testClass1 = new BoolTestClass();
            testClass1.TestBool4 = !testClass1.TestBool4;
            testClass1.TestBool5 = !testClass1.TestBool5;
            testClass1.TestBool6 = !testClass1.TestBool6;

            string json = testClass1.SerializeToJson();

            BoolTestClass testClass2 = new BoolTestClass();// JsonSerializer.Deserialize<BoolTestClass>(json1);
            testClass2.UpdateFromJson<BoolTestClass>(json);

            Assert.AreEqual(testClass1.TestBool1, testClass2.TestBool1, "TestBool1 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.TestBool2, testClass2.TestBool2, "TestBool2 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.TestBool3, testClass2.TestBool3, "TestBool3 should be equal after serialization and deserialization.");
            Assert.AreNotEqual(testClass1.TestBool4, testClass2.TestBool4, "TestBool4 should not be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.TestBool5, testClass2.TestBool5, "TestBool5 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.TestBool6, testClass2.TestBool6, "TestBool6 should be equal after serialization and deserialization.");
        }

        [Test]
        public void TestBoolWithRangeAttribute()
        {
            BoolTestBadAttribute testClass1 = new BoolTestBadAttribute();
            testClass1.SerializeToJson();
            ParameterModelFactory parameterModelFactory = new ParameterModelFactory(new VariablesContext());
            try
            {
                Dictionary<string, IParameterModel> testModels1 = parameterModelFactory.GetModels(testClass1);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("Range"), "Expected Range exception for bool property.");
            }
        }

        [Test]
        public void TestBoolAssignment()
        {
            BoolTestClass testClass1 = new BoolTestClass();
            testClass1.TestBool1 = false;
            testClass1.TestBool3 = false;
            testClass1.TestBool5 = false;
            IVariablesContext variablesContext = new VariablesContext();
            ParameterModelFactory parameterModelFactory = new ParameterModelFactory(variablesContext);

            bool testResult = testClass1.TestPropertyValue("TestBool1", "true", out string error);
            Assert.IsTrue(testResult, "TestPropertyValue should return true for valid boolean assignment.");

            testResult = testClass1.TrySetPropertyValue("TestBool1", "true", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true for valid boolean assignment.");
            Assert.IsTrue(testClass1.TestBool1, "TestBool1 should be true after assignment.");

            testResult = testClass1.TestPropertyValue("TestBool1", "dsdfg", out error);
            Assert.IsFalse(testResult, "TestPropertyValue should return false for invalid boolean assignment.");

            testResult = testClass1.TestPropertyValue("TestBool3", "true", out error);
            Assert.IsFalse(testResult, "TestPropertyValue should return false for invalid boolean assignment.");

            //testResult = testClass1.TrySetVariableValue("TestBool3", "true", out error);
            //Assert.IsFalse(testResult, "TrySetVariableValue should return true for valid boolean assignment.");

            testResult = testClass1.TestPropertyValue("TestBool5", "true", out error);
            Assert.IsFalse(testResult, "TestPropertyValue should return false for invalid boolean assignment.");

            testResult = testClass1.TrySetPropertyValue("TestBool5", "true", out error);
            Assert.IsFalse(testResult, "TrySetVariableValue should return true for valid boolean assignment.");

            //testResult = testClass1.TrySetVariableValue("TestBool3", "true", out error);
            //Assert.IsFalse(testResult, "TrySetVariableValue should return true for valid boolean assignment.");
        }

        [Test]
        public void TestBoolVariableImplementation()
        {
            BoolTestClass testClass1 = new BoolTestClass();
            IVariablesContext variablesContext = new VariablesContext();
            variablesContext.AddVariable("boolVar1", VariableType.Boolean, VariableSource.UserDefined);
            variablesContext.AddVariable("boolVar2", VariableType.Boolean, VariableSource.UserDefined);
            variablesContext.AddVariable("intVar1", VariableType.Integer, VariableSource.UserDefined);
            ParameterModelFactory parameterModelFactory = new ParameterModelFactory(variablesContext);

            bool testResult = testClass1.TrySetVariableValue("TestBool1", "boolVar1", out string error);
            Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestBool1' can not be set to a variable.");

            testResult = testClass1.TrySetVariableValue("TestBool2", "boolVar1", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true because 'TestBool2' can be set to a variable.");

            testResult = testClass1.TrySetVariableValue("TestBool2", "sdfsdf", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true.");

            testResult = testClass1.TrySetVariableValue("TestBool2", "intVar1", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true.");

            Dictionary<string, string> resolveErrors = new Dictionary<string, string>();
            testResult = testClass1.TryResolveVariables(variablesContext, resolveErrors);
            Assert.IsFalse(testResult, "TryResolveVariables should return false because 'TestBool2' can not be assigned 'intVar1'.");
            Assert.IsTrue(resolveErrors.ContainsKey("TestBool2"), "Resolve errors should contain 'TestBool2'.");
        }
        #endregion Boolean tests


        #region Integer and float tests
        internal class IntAndFloatTestClass : ImplementsParametersBase
        {
            // Should pass.
            [Parameter]
            public int TestInt1 { get; set; } = 1;

            // Should pass.
            [Parameter(CanBeVariable = true)]
            public int TestInt2 { get; set; } = 2;

            // Should pass.
            [Parameter(CanBeVariable = true)]
            [Editable(false)]
            public int TestInt3 { get; set; } = 3;

            // Not a parameter.
            public int TestInt4 { get; set; } = 4;
            // Should pass.

            [Parameter]
            [Editable(false)]
            public int TestInt5 { get; set; } = 5;

            // Should pass.
            [Parameter]
            public float TestFloat1 { get; set; } = 1;

            // Should pass.
            [Parameter(CanBeVariable = true)]
            public float TestFloat2 { get; set; } = 2;

            // Should pass.
            [Parameter(CanBeVariable = true)]
            [Editable(false)]
            public float TestFloat3 { get; set; } = 3;

            // Not a parameter.
            public float TestFloat4 { get; set; } = 4;
            // Should pass.

            [Parameter]
            [Editable(false)]
            public float TestFloat5 { get; set; } = 5;

        }

        internal class IntAndFloatTestRangeAttribute : ImplementsParametersBase
        {
            [Parameter]
            [Range(2, 4)]
            public int intRange2_4 { get; set; } = 2;

            [Parameter]
            [Range(1, 8)]
            public int intRange1_8 { get; set; } = 10;

            [Parameter]
            [Range(5, 10)]
            public float floatRange5_10 { get; set; } = 6;

            [Parameter]
            [Range(4, 11)]
            public float floatRange4_11 { get; set; } = 20;
        }


        [Test]
        public void TestIntAndFloatSerialize()
        {
            IntAndFloatTestClass testClass1 = new IntAndFloatTestClass();
            testClass1.TestInt1++;
            testClass1.TestInt2++;
            testClass1.TestInt3++;
            testClass1.TestInt4++;
            testClass1.TestInt5++;

            testClass1.TestFloat1++;
            testClass1.TestFloat2++;
            testClass1.TestFloat3++;
            testClass1.TestFloat4++;
            testClass1.TestFloat5++;

            string json = testClass1.SerializeToJson();

            IntAndFloatTestClass testClass2 = new IntAndFloatTestClass();// JsonSerializer.Deserialize<BoolTestClass>(json1);
            testClass2.UpdateFromJson<BoolTestClass>(json);

            Assert.AreEqual(testClass1.TestInt1, testClass2.TestInt1, "TestInt1 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.TestInt2, testClass2.TestInt2, "TestInt2 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.TestInt3, testClass2.TestInt3, "TestInt3 should be equal after serialization and deserialization.");
            Assert.AreNotEqual(testClass1.TestInt4, testClass2.TestInt4, "TestInt4 should not be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.TestInt5, testClass2.TestInt5, "TestInt5 should be equal after serialization and deserialization.");

            Assert.AreEqual(testClass1.TestFloat1, testClass2.TestFloat1, "TestFloat1 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.TestFloat2, testClass2.TestFloat2, "TestFloat2 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.TestFloat3, testClass2.TestFloat3, "TestFloat3 should be equal after serialization and deserialization.");
            Assert.AreNotEqual(testClass1.TestFloat4, testClass2.TestFloat4, "TestFloat4 should not be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.TestFloat5, testClass2.TestFloat5, "TestFloat5 should be equal after serialization and deserialization.");
        }

        [Test]
        public void TestIntAndFloatWithRangeAttribute()
        {
            IntAndFloatTestRangeAttribute testClass1 = new IntAndFloatTestRangeAttribute();
            testClass1.SerializeToJson();
            ParameterModelFactory parameterModelFactory = new ParameterModelFactory(new VariablesContext());
            try
            {
                Dictionary<string, IParameterModel> testModels1 = parameterModelFactory.GetModels(testClass1);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("Range"), "Expected Range exception for bool property.");
            }
        }

        [Test]
        public void TestIntAndFloatAssignment()
        {
            BoolTestClass testClass1 = new BoolTestClass();
            testClass1.TestBool1 = false;
            IVariablesContext variablesContext = new VariablesContext();
            ParameterModelFactory parameterModelFactory = new ParameterModelFactory(variablesContext);

            bool testResult = testClass1.TestPropertyValue("TestBool1", "true", out string error);
            Assert.IsTrue(testResult, "TestPropertyValue should return true for valid boolean assignment.");

            testResult = testClass1.TrySetPropertyValue("TestBool1", "true", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true for valid boolean assignment.");
            Assert.IsTrue(testClass1.TestBool1, "TestBool1 should be true after assignment.");

            testResult = testClass1.TestPropertyValue("TestBool1", "dsdfg", out error);
            Assert.IsFalse(testResult, "TestPropertyValue should return false for invalid boolean assignment.");
        }

        [Test]
        public void TestIntAndFloatVariableImplementation()
        {
            BoolTestClass testClass1 = new BoolTestClass();
            IVariablesContext variablesContext = new VariablesContext();
            variablesContext.AddVariable("boolVar1", VariableType.Boolean, VariableSource.UserDefined);
            variablesContext.AddVariable("boolVar2", VariableType.Boolean, VariableSource.UserDefined);
            variablesContext.AddVariable("intVar1", VariableType.Integer, VariableSource.UserDefined);
            ParameterModelFactory parameterModelFactory = new ParameterModelFactory(variablesContext);

            bool testResult = testClass1.TrySetVariableValue("TestBool1", "boolVar1", out string error);
            Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestBool1' can not be set to a variable.");

            testResult = testClass1.TrySetVariableValue("TestBool2", "boolVar1", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true because 'TestBool2' can be set to a variable.");

            testResult = testClass1.TrySetVariableValue("TestBool2", "sdfsdf", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true.");

            testResult = testClass1.TrySetVariableValue("TestBool2", "intVar1", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true.");

            Dictionary<string, string> resolveErrors = new Dictionary<string, string>();
            testResult = testClass1.TryResolveVariables(variablesContext, resolveErrors);
            Assert.IsFalse(testResult, "TryResolveVariables should return false because 'TestBool2' can not be assigned 'intVar1'.");
            Assert.IsTrue(resolveErrors.ContainsKey("TestBool2"), "Resolve errors should contain 'TestBool2'.");
        }
        #endregion Integer and float tests

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


        //#region 
        //protected RequiredAttribute _requiredAttribute; //Ensures that a property cannot be null or empty.It's commonly used to indicate mandatory fields. 
        //protected StringLengthAttribute _stringLengthAttribute; //Specifies the minimum and maximum length of a string property. 
        //protected RangeAttribute _rangeAttribute; //Limits a numeric property to a specific range of values. 
        //protected EmailAddressAttribute _emailAddressAttribute; //Validates that a string property is a valid email address. 
        //protected PhoneAttribute _phoneAttribute; //Validates that a string property is a valid phone number. 
        //protected UrlAttribute _urlAttribute; //Validates that a string property is a valid URL.
        //protected DataTypeAttribute _dataTypeAttribute; //Provides metadata about the data type of a property, such as Date, Time, PhoneNumber, Currency, etc.
        //protected EnumDataTypeAttribute _enumDataTypeAttribute; //Links an enum to a data column, ensuring the property's value is within the enum's range.
        //protected FileExtensionsAttribute _fileExtensionsAttribute; //Validates that a file name extension is valid.
        //protected CustomValidationAttribute _customValidationAttribute; //Allows for custom validation logic to be applied to a property.
        //protected DisplayAttribute _displayAttribute; // Specifies localizable strings for display purposes, such as the field's name, description, or prompt.
        //protected DisplayFormatAttribute _displayFormatAttribute; // Controls how data fields are displayed and formatted in user interfaces.
        //protected EditableAttribute _editableAttribute; // Indicates whether a property is editable or read-only
        //#endregion


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