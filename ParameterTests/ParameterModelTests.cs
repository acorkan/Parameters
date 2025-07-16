using ParameterModel.Attributes;
using ParameterModel.Extensions;
using ParameterModel.Factories;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System.ComponentModel.DataAnnotations;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace ParameterTests
{
    [TestFixture]
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

        private ParameterModelFactory _parameterModelFactory;
        private IVariablesContext _variablesContext;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _variablesContext = new VariablesContext();
            _parameterModelFactory = new ParameterModelFactory(_variablesContext);
        }

        [SetUp]
        public void SetUp()
        {
            _variablesContext.ClearVariables();
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

            bool testResult = testClass1.TryAssignVariable("TestBool1", "boolVar1", out string error);
            Assert.IsFalse(testResult, "TrySetVariableValue should return false because 'TestBool1' can not be set to a variable.");

            testResult = testClass1.TryAssignVariable("TestBool2", "boolVar1", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true because 'TestBool2' can be set to a variable.");

            testResult = testClass1.TryAssignVariable("TestBool2", "sdfsdf", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true.");

            testResult = testClass1.TryAssignVariable("TestBool2", "intVar1", out error);
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
            testClass2.UpdateFromJson<IntAndFloatTestClass>(json);

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
            Assert.DoesNotThrow(() => _parameterModelFactory.GetModels(testClass1), "No Range exception for int or float property.");
        }

        [Test]
        public void TestValidateIntAndFloatInitialValue()
        {
            IntAndFloatTestRangeAttribute testClass1 = new IntAndFloatTestRangeAttribute();
            Dictionary<string,List<string>> errors = new Dictionary<string, List<string>>();
            Assert.IsFalse(testClass1.TryValidateParameters(null, errors));
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

            Dictionary<string, string> resolveErrors = new Dictionary<string, string>();
            testResult = testClass1.TryResolveVariables(_variablesContext, resolveErrors);
            Assert.IsTrue(testResult, "TryResolveVariables should return true because no violations.");

            testResult = testClass1.TryAssignVariable("TestInt2", "floatVar1", out error);
            Assert.IsTrue(testResult, "TrySetVariableValue should return true because 'TestInt2' can be assigned to a float variable.");

            //testResult = testClass1.TryAssignVariable("TestBool2", "sdfsdf", out error);
            //Assert.IsTrue(testResult, "TrySetVariableValue should return true.");

            //testResult = testClass1.TryAssignVariable("TestBool2", "intVar1", out error);
            //Assert.IsTrue(testResult, "TrySetVariableValue should return true.");

            testResult = testClass1.TryResolveVariables(_variablesContext, resolveErrors);
            Assert.IsFalse(testResult, "TryResolveVariables should return false because 'TestInt2' can not be resolved to 'floatVar1'.");
            Assert.IsTrue(resolveErrors.ContainsKey("TestInt2"), "Resolve errors should contain 'TestBool2'.");
        }
        #endregion Integer and float tests

        #region Enum tests
        #endregion Enum tests
    }
}