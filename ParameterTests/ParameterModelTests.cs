using ParameterModel.Attributes;
using ParameterModel.Extensions;
using ParameterModel.Factories;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using ParameterModel.Variables;
using ParameterTests.TestClasses;
using System.ComponentModel.DataAnnotations;
using Windows.Foundation.Metadata;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace ParameterTests
{
    [TestFixture]
    public class ParameterModelTests
    {
        //internal enum OptionEnum
        //{
        //    [System.ComponentModel.Description("Option 1 Description")]
        //    Option1 = 1,
        //    [System.ComponentModel.Description("Option 2 Description")]
        //    Option2 = 2,
        //    [System.ComponentModel.Description("Option 3 Description")]
        //    Option3 = 3,
        //    Option4 = 4,
        //    Option5 = 5
        //}

        //internal class ImplementsParametersBase : IImplementsParameterAttribute
        //{
        //    public Dictionary<string, string> VariableAssignments { get; } = new Dictionary<string, string>();
        //    public Dictionary<string, ParameterAttribute> AttributeMap { get; } = new Dictionary<string, ParameterAttribute>();
        //}

        private ParameterModelFactory _parameterModelFactory;
        private IVariablesContext _variablesContext;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _variablesContext = new VariablesContext();
            _parameterModelFactory = new ParameterModelFactory();
        }

        [SetUp]
        public void SetUp()
        {
            _variablesContext.ClearVariables();
        }

        #region Boolean tests


     

        //[Test]
        //public void TestBoolVariableImplementation()
        //{
        //    BoolTestClass testClass1 = new BoolTestClass();
        //    IVariablesContext variablesContext = new VariablesContext();
        //    variablesContext.AddVariable("boolVar1", VariableType.Boolean, VariableSource.UserDefined);
        //    variablesContext.AddVariable("boolVar2", VariableType.Boolean, VariableSource.UserDefined);
        //    variablesContext.AddVariable("intVar1", VariableType.Integer, VariableSource.UserDefined);
        //    ParameterModelFactory parameterModelFactory = new ParameterModelFactory(variablesContext);

        //    bool testResult = testClass1.TryAssignVariable("Bool1", "boolVar1", out string error);
        //    Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestBool1' can not be set to a variable.");

        //    testResult = testClass1.TryAssignVariable("Bool2", "boolVar1", out error);
        //    Assert.IsTrue(testResult, "TrySetVariableValue should return true because 'TestBool2' can be set to a variable.");

        //    testResult = testClass1.TryAssignVariable("Bool2", "sdfsdf", out error);
        //    Assert.IsTrue(testResult, "TrySetVariableValue should return true.");

        //    testResult = testClass1.TryAssignVariable("Bool2", "intVar1", out error);
        //    Assert.IsTrue(testResult, "TrySetVariableValue should return true.");

        //    Dictionary<string, string> resolveErrors = new Dictionary<string, string>();
        //    testResult = testClass1.ResolveVariables(variablesContext, resolveErrors);
        //    Assert.IsFalse(testResult, "TryResolveVariables should return false because 'TestBool2' can not be assigned 'intVar1'.");
        //    Assert.IsTrue(resolveErrors.ContainsKey("Bool2"), "Resolve errors should contain 'TestBool2'.");
        //}
        #endregion Boolean tests

/*
        #region Integer and float tests
        internal class IntAndFloatTestClass : ImplementsParametersBase
        {
            // Should pass.
            [Parameter]
            public int TestInt1 { get; set; } = 1;

            // Should pass.
            [Parameter(true)]
            public int TestInt2 { get; set; } = 2;

            // Should pass.
            [Parameter(true)]
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
            [Parameter(true)]
            public float TestFloat2 { get; set; } = 2;

            // Should pass.
            [Parameter(true)]
            [Editable(false)]
            public float TestFloat3 { get; set; } = 3;

            // Not a parameter.
            public float TestFloat4 { get; set; } = 4;
            // Should pass.

            [Parameter]
            [Editable(false)]
            [Range(0.0, 10.0)]
            public float TestFloat5 { get; set; } = 5;
        }


        internal class IntAndFloatBadAttributesTestClass : ImplementsParametersBase
        {
            [Parameter]
            [Editable(false)]
            [Range(0.0, 10.0)]
            [Phone]
            [StringLength(5)]
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

            string json = testClass1.SerializeParametersToJson();

            IntAndFloatTestClass testClass2 = new IntAndFloatTestClass();// JsonSerializer.Deserialize<BoolTestClass>(json1);
            testClass2.UpdateParametersFromJson<IntAndFloatTestClass>(json);

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
            testClass1.SerializeParametersToJson();
            Assert.DoesNotThrow(() => _parameterModelFactory.GetModels(testClass1), "No Range exception for int or float property.");
        }

        [Test]
        public void TestValidateIntAndFloatInitialValue()
        {
            IntAndFloatTestRangeAttribute testClass1 = new IntAndFloatTestRangeAttribute();
            Dictionary<string,List<string>> errors = new Dictionary<string, List<string>>();
            Assert.IsFalse(testClass1.ValidateParameters(null, errors));
            Assert.IsTrue(errors.Count == 2);
            Assert.IsTrue(errors.ContainsKey("intRange1_8"));
            Assert.IsTrue(errors.ContainsKey("floatRange4_11"));
        }

        [Test]
        public void TestIntAndFloatAssignment()
        {
            IntAndFloatTestClass testClass1 = new IntAndFloatTestClass();

            testClass1.TestInt1 = 10;
            bool testResult = testClass1.TestPropertyValue("TestInt1", "12", out string error);
            Assert.IsTrue(testResult, "TestPropertyValue should return true for valid int assignment.");

            testResult = testClass1.TestPropertyValue("TestInt1", "10.0", out error);
            Assert.IsFalse(testResult, "TestPropertyValue should return true for valid int assignment.");

            testClass1.TestInt3 = 10;
            testResult = testClass1.TestPropertyValue("TestInt3", "14", out error);
            Assert.IsFalse(testResult, "TestPropertyValue should return false for read-only property.");

            testClass1.TestFloat1 = 88.4F;
            testResult = testClass1.TestPropertyValue("TestFloat1", "77", out error);
            Assert.IsTrue(testResult, "TestPropertyValue should return true for valid float assignment.");

            testResult = testClass1.TestPropertyValue("TestFloat1", "909.9", out error);
            Assert.IsTrue(testResult, "TestPropertyValue should return true for valid float assignment.");
        }

        [Test]
        public void TestIntAndFloatVariableImplementation()
        {
            IntAndFloatTestClass testClass1 = new IntAndFloatTestClass();
            _variablesContext.AddVariable("intVar1", VariableType.Integer, VariableSource.UserDefined).SetValue(11);
            //_variablesContext.AddVariable("intVar2", VariableType.Integer, VariableSource.UserDefined).SetValue(22);

            _variablesContext.AddVariable("floatVar1", VariableType.Float, VariableSource.UserDefined).SetValue(33.3F);
            //_variablesContext.AddVariable("floatVar2", VariableType.Float, VariableSource.UserDefined).SetValue(44.4F);

            bool testResult = testClass1.TryAssignVariable("TestInt1", "intVar1", out string error);
            Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestInt1' can not be set to a variable.");

            testResult = testClass1.TryAssignVariable("TestInt3", "intVar1", out error);
            Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestInt5' is read-only.");

            testResult = testClass1.TryAssignVariable("TestInt2", "intVar1", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true because 'TestInt2' can be set to a variable.");


            testResult = testClass1.TryAssignVariable("TestFloat1", "floatVar1", out error);
            Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestFloat1' can not be set to a variable.");

            testResult = testClass1.TryAssignVariable("TestFloat3", "floatVar1", out error);
            Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestFloat3' is read-only.");

            testResult = testClass1.TryAssignVariable("TestFloat2", "floatVar1", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true because 'TestFloat2' can be set to a variable.");


            testResult = testClass1.TryAssignVariable("TestInt2", "floatVar1", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true because 'TestInt2' can be assigned to a float variable.");

            Dictionary<string, string> resolveErrors = new Dictionary<string, string>();

            testResult = testClass1.ResolveVariables(_variablesContext, resolveErrors);
            Assert.IsFalse(testResult, "TryResolveVariables should return false because 'TestInt2' can not be resolved to 'floatVar1'.");
            Assert.IsTrue(resolveErrors.ContainsKey("TestInt2"), "Resolve errors should contain 'TestBool2'.");
        }


        //Dictionary<string, string> resolveErrors = new Dictionary<string, string>();
        //try
        //{
        //    testResult = testClass1.TryResolveVariables(_variablesContext, resolveErrors);
        //}
        //catch (Exception ex)
        //{
        //    Assert.IsTrue(ex.Message.Contains("testFloat2"), "TryResolveVariables exception should contain 'testFloat2'.");
        //    Assert.IsTrue(ex.Message.Contains("StringLength"), "TryResolveVariables exception should contain 'StringLength'.");
        //    Assert.IsTrue(ex.Message.Contains("Phone"), "TryResolveVariables exception should contain 'Phone'.");
        //}

        [Test]
        public void TestFloatWithBadAttribute()
        {
            IntAndFloatBadAttributesTestClass testClass1 = new IntAndFloatBadAttributesTestClass();
            try
            {
                testClass1.SerializeParametersToJson();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("StringLength"), "Expected StringLength exception for float property.");
                Assert.IsTrue(ex.Message.Contains("Phone"), "Expected Phone exception for float property.");
            }
        }

        #endregion Integer and float tests

        #region String and string[] tests
        #endregion String and string[] tests
        #region Enum tests
        #endregion Enum tests
        #region VariableType tests
        internal class VariableTypeTestClass : ImplementsParametersBase
        {
            // Should pass.
            [Parameter]
            public int Test1 { get; set; } = 1;

            // Should pass.
            [Parameter(true)]
            public int Test2 { get; set; } = 2;

            // Must be string.
            [VariableAssignment(VariableType.Integer)]
            public int Test3 { get; set; } = 3;

            // Not a parameter.
            public int Test4 { get; set; } = 4;
            // Should pass.

            [VariableAssignment(VariableType.Integer)]
            public Variable Test5 { get; set; } = new Variable(";oesep;ijk");

            // Unallowed attributes.
            [VariableAssignment(VariableType.Integer)]
            public Variable Test6 { get; set; } = new Variable("dfzhjkl");
        }
        

        //internal class IntAndFloatBadAttributesTestClass : ImplementsParametersBase
        //{
        //    [Parameter]
        //    [Editable(false)]
        //    [Range(0.0, 10.0)]
        //    [Phone]
        //    [StringLength(5)]
        //    public float TestFloat5 { get; set; } = 5;
        //}

        //internal class IntAndFloatTestRangeAttribute : ImplementsParametersBase
        //{
        //    [Parameter]
        //    [Range(2, 4)]
        //    public int intRange2_4 { get; set; } = 2;

        //    [Parameter]
        //    [Range(1, 8)]
        //    public int intRange1_8 { get; set; } = 10;

        //    [Parameter]
        //    [Range(5, 10)]
        //    public float floatRange5_10 { get; set; } = 6;

        //    [Parameter]
        //    [Range(4, 11)]
        //    public float floatRange4_11 { get; set; } = 20;
        //}


        [Test]
        public void TestVariableTypeSerialize()
        {
            VariableTypeTestClass testClass1 = new VariableTypeTestClass();
            testClass1.Test1++;
            testClass1.Test2++;
            testClass1.Test3++;
            testClass1.Test4++;
            testClass1.Test5.Assignment = "test5";
            testClass1.Test6.Assignment = "test6__";

            string json = testClass1.SerializeParametersToJson();

            VariableTypeTestClass testClass2 = new VariableTypeTestClass();// JsonSerializer.Deserialize<BoolTestClass>(json1);
            testClass2.UpdateParametersFromJson<VariableTypeTestClass>(json);

            Assert.AreEqual(testClass1.Test1, testClass2.Test1, "Test1 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Test2, testClass2.Test2, "Test2 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Test3, testClass2.Test3, "Test3 should be equal after serialization and deserialization.");
            Assert.AreNotEqual(testClass1.Test4, testClass2.Test4, "Test4 should not be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Test5, testClass2.Test5, "Test5 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Test6, testClass2.Test6, "Test5 should be equal after serialization and deserialization.");
        }

        //[Test]
        //public void TestIntAndFloatWithRangeAttribute()
        //{
        //    IntAndFloatTestRangeAttribute testClass1 = new IntAndFloatTestRangeAttribute();
        //    testClass1.SerializeToJson();
        //    Assert.DoesNotThrow(() => _parameterModelFactory.GetModels(testClass1), "No Range exception for int or float property.");
        //}

        //[Test]
        //public void TestValidateIntAndFloatInitialValue()
        //{
        //    IntAndFloatTestRangeAttribute testClass1 = new IntAndFloatTestRangeAttribute();
        //    Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        //    Assert.IsFalse(testClass1.TryValidateParameters(null, errors));
        //    Assert.IsTrue(errors.Count == 2);
        //    Assert.IsTrue(errors.ContainsKey("intRange1_8"));
        //    Assert.IsTrue(errors.ContainsKey("floatRange4_11"));
        //}

        //[Test]
        //public void TestIntAndFloatAssignment()
        //{
        //    IntAndFloatTestClass testClass1 = new IntAndFloatTestClass();

        //    testClass1.TestInt1 = 10;
        //    bool testResult = testClass1.TestPropertyValue("TestInt1", "12", out string error);
        //    Assert.IsTrue(testResult, "TestPropertyValue should return true for valid int assignment.");

        //    testResult = testClass1.TestPropertyValue("TestInt1", "10.0", out error);
        //    Assert.IsFalse(testResult, "TestPropertyValue should return true for valid int assignment.");

        //    testClass1.TestInt3 = 10;
        //    testResult = testClass1.TestPropertyValue("TestInt3", "14", out error);
        //    Assert.IsFalse(testResult, "TestPropertyValue should return false for read-only property.");

        //    testClass1.TestFloat1 = 88.4F;
        //    testResult = testClass1.TestPropertyValue("TestFloat1", "77", out error);
        //    Assert.IsTrue(testResult, "TestPropertyValue should return true for valid float assignment.");

        //    testResult = testClass1.TestPropertyValue("TestFloat1", "909.9", out error);
        //    Assert.IsTrue(testResult, "TestPropertyValue should return true for valid float assignment.");
        //}

        //[Test]
        //public void TestIntAndFloatVariableImplementation()
        //{
        //    IntAndFloatTestClass testClass1 = new IntAndFloatTestClass();
        //    _variablesContext.AddVariable("intVar1", VariableType.Integer, VariableSource.UserDefined).SetValue(11);
        //    //_variablesContext.AddVariable("intVar2", VariableType.Integer, VariableSource.UserDefined).SetValue(22);

        //    _variablesContext.AddVariable("floatVar1", VariableType.Float, VariableSource.UserDefined).SetValue(33.3F);
        //    //_variablesContext.AddVariable("floatVar2", VariableType.Float, VariableSource.UserDefined).SetValue(44.4F);

        //    bool testResult = testClass1.TryAssignVariable("TestInt1", "intVar1", out string error);
        //    Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestInt1' can not be set to a variable.");

        //    testResult = testClass1.TryAssignVariable("TestInt3", "intVar1", out error);
        //    Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestInt5' is read-only.");

        //    testResult = testClass1.TryAssignVariable("TestInt2", "intVar1", out error);
        //    Assert.IsTrue(testResult, "TrySetVariableValue should return true because 'TestInt2' can be set to a variable.");


        //    testResult = testClass1.TryAssignVariable("TestFloat1", "floatVar1", out error);
        //    Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestFloat1' can not be set to a variable.");

        //    testResult = testClass1.TryAssignVariable("TestFloat3", "floatVar1", out error);
        //    Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestFloat3' is read-only.");

        //    testResult = testClass1.TryAssignVariable("TestFloat2", "floatVar1", out error);
        //    Assert.IsTrue(testResult, "TrySetVariableValue should return true because 'TestFloat2' can be set to a variable.");


        //    testResult = testClass1.TryAssignVariable("TestInt2", "floatVar1", out error);
        //    Assert.IsTrue(testResult, "TrySetVariableValue should return true because 'TestInt2' can be assigned to a float variable.");

        //    Dictionary<string, string> resolveErrors = new Dictionary<string, string>();

        //    testResult = testClass1.TryResolveVariables(_variablesContext, resolveErrors);
        //    Assert.IsFalse(testResult, "TryResolveVariables should return false because 'TestInt2' can not be resolved to 'floatVar1'.");
        //    Assert.IsTrue(resolveErrors.ContainsKey("TestInt2"), "Resolve errors should contain 'TestBool2'.");
        //}


        ////Dictionary<string, string> resolveErrors = new Dictionary<string, string>();
        ////try
        ////{
        ////    testResult = testClass1.TryResolveVariables(_variablesContext, resolveErrors);
        ////}
        ////catch (Exception ex)
        ////{
        ////    Assert.IsTrue(ex.Message.Contains("testFloat2"), "TryResolveVariables exception should contain 'testFloat2'.");
        ////    Assert.IsTrue(ex.Message.Contains("StringLength"), "TryResolveVariables exception should contain 'StringLength'.");
        ////    Assert.IsTrue(ex.Message.Contains("Phone"), "TryResolveVariables exception should contain 'Phone'.");
        ////}

        //[Test]
        //public void TestFloatWithBadAttribute()
        //{
        //    IntAndFloatBadAttributesTestClass testClass1 = new IntAndFloatBadAttributesTestClass();
        //    try
        //    {
        //        testClass1.SerializeToJson();
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.IsTrue(ex.Message.Contains("StringLength"), "Expected StringLength exception for float property.");
        //        Assert.IsTrue(ex.Message.Contains("Phone"), "Expected Phone exception for float property.");
        //    }
        //}

        #endregion VariableType tests
*/
    }
}