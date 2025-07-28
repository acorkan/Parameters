using ParameterModel.Extensions;
using ParameterModel.Factories;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using ParameterTests.TestClasses;
using System.Text.Json;

namespace ParameterTests.Tests
{
    [TestFixture]
    internal class BoolParameterModelTests
    {
        protected ParameterModelFactory _parameterModelFactory;
        protected IVariablesContext _variablesContext;

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

        [Test]
        public void TestBoolSerializeParameters()
        {
            BoolTestClass testClass1 = new BoolTestClass();
            testClass1.Bool1 = !testClass1.Bool1;
            testClass1.Bool4 = !testClass1.Bool4;
            testClass1.Bool5 = !testClass1.Bool5;

            string json = testClass1.SerializeParametersToJson();

            BoolTestClass testClass2 = new BoolTestClass();
            testClass2.UpdateParametersFromJson<BoolTestClass>(json);

            Assert.AreEqual(testClass1.Bool1, testClass2.Bool1, "Bool1 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool2, testClass2.Bool2, "Bool2 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool3, testClass2.Bool3, "Bool3 should be equal after serialization and deserialization.");
            Assert.AreNotEqual(testClass1.Bool4, testClass2.Bool4, "Bool4 should not be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool5, testClass2.Bool5, "Bool5 should be equal after serialization and deserialization.");

            //Assert.AreEqual(testClass1.GetAssignedVariable("Bool3"), "BoolVar2", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            //Assert.AreEqual(testClass1.GetAssignedVariable("Bool2"), "BoolVar1", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            //Assert.AreEqual(testClass2.GetAssignedVariable("Bool3"), "BoolVar2", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            //Assert.AreEqual(testClass2.GetAssignedVariable("Bool2"), "BoolVar1", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
        }

        [Test]
        public void TestBoolSerializeParametersWithVariables()
        {
            _variablesContext.AddVariable("BoolVar1", VariableType.Boolean).SetValue(true);
            _variablesContext.AddVariable("BoolVar2", VariableType.Boolean).SetValue(false);

            BoolTestClass testClass1 = new BoolTestClass();

            bool testResult;
            testClass1.TryAssignVariable(_variablesContext, "Bool3", "BoolVar2", out string error);
            testClass1.TryAssignVariable(_variablesContext, "Bool2", "BoolVar1", out error);

            string json = testClass1.SerializeParametersToJson();

            BoolTestClass testClass2 = new BoolTestClass();
            testClass2.UpdateParametersFromJson<BoolTestClass>(json);
            Assert.That("BoolVar2", Is.EqualTo(testClass1.VariableAssignments["Bool3"]));
            Assert.That("BoolVar1", Is.EqualTo(testClass1.VariableAssignments["Bool2"]));
            Assert.That("BoolVar2", Is.EqualTo(testClass2.VariableAssignments["Bool3"]));
            Assert.That("BoolVar1", Is.EqualTo(testClass2.VariableAssignments["Bool2"]));
            //Assert.AreEqual(testClass1.Bool1, testClass2.Bool1, "Bool1 should be equal after serialization and deserialization.");
            //Assert.AreEqual(testClass1.Bool2, testClass2.Bool2, "Bool2 should be equal after serialization and deserialization.");
            //Assert.AreEqual(testClass1.Bool3, testClass2.Bool3, "Bool3 should be equal after serialization and deserialization.");
            //Assert.AreNotEqual(testClass1.Bool4, testClass2.Bool4, "Bool4 should not be equal after serialization and deserialization.");
            //Assert.AreEqual(testClass1.Bool5, testClass2.Bool5, "Bool5 should be equal after serialization and deserialization.");
        }

        [Test]
        public void TestBoolGenericSerialize()
        {
            _variablesContext.AddVariable("BoolVar1", VariableType.Boolean).SetValue(true);
            _variablesContext.AddVariable("BoolVar2", VariableType.Boolean).SetValue(false);

            BoolTestClass testClass1 = new BoolTestClass();
            testClass1.Bool1 = !testClass1.Bool1;
            testClass1.Bool4 = !testClass1.Bool4;
            testClass1.Bool5 = !testClass1.Bool5;

            bool testResult;
            testClass1.TryAssignVariable(_variablesContext, "Bool3", "BoolVar2", out string error);
            testClass1.TryAssignVariable(_variablesContext, "Bool2", "BoolVar1", out error);

            string json = JsonSerializer.Serialize(testClass1);

            BoolTestClass testClass2 = JsonSerializer.Deserialize<BoolTestClass>(json);

            Assert.AreEqual(testClass1.Bool1, testClass2.Bool1, "Bool1 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool2, testClass2.Bool2, "Bool2 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool3, testClass2.Bool3, "Bool3 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool4, testClass2.Bool4, "Bool4 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool5, testClass2.Bool5, "Bool5 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.GetAssignedVariable("Bool3"), "BoolVar2", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            Assert.AreEqual(testClass1.GetAssignedVariable("Bool2"), "BoolVar1", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            Assert.AreEqual(testClass2.GetAssignedVariable("Bool3"), "BoolVar2", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            Assert.AreEqual(testClass2.GetAssignedVariable("Bool2"), "BoolVar1", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
        }


        [Test]
        public void TestBoolWithRangeAttribute()
        {
            BoolTestBadAttribute testClass1 = null;
            Assert.Throws<NotSupportedException>(() => testClass1 = new BoolTestBadAttribute(), "BoolTestBadAttribute should throw an exception due to Range and StringLength attributes on a bool property.");
        }

        [Test]
        public void TestBoolAssignment()
        {
            BoolTestClass testClass1 = new BoolTestClass();
            testClass1.Bool1 = false;
            testClass1.Bool3 = false;
            testClass1.Bool5 = false;

            bool testResult = testClass1.TestSetParameter("Bool1", "true");
            Assert.IsTrue(testResult, "TestSetParameter should return true for valid boolean assignment.");

            testResult = testClass1.TrySetParameter("Bool1", "true");
            Assert.IsTrue(testResult, "TrySetVariableValue should return true for valid boolean assignment.");
            Assert.IsTrue(testClass1.Bool1, "Bool1 should be true after assignment.");

            testResult = testClass1.TestSetParameter("Bool1", "dsdfg");
            Assert.IsFalse(testResult, "TestSetParameter should return false for invalid boolean assignment.");

            Assert.Throws<ArgumentException>(() => testClass1.TestSetParameter("Bool4", "dsdfg"), "TestSetParameter should throw an exception for invalid boolean parameter.");

            testResult = testClass1.TrySetParameter("Bool5", "true");
            Assert.IsTrue(testResult, "TestSetParameter should return true for read-only parameter assigned in code.");
            Assert.AreEqual(testClass1.Bool5, true);
        }

        [Test]
        public void TestVariablesContextAddVariable()
        {
            _variablesContext.AddVariable("BoolVar1", VariableType.Boolean).SetValue(true);
            Assert.Throws<InvalidOperationException>(() => _variablesContext.AddVariable("FloatVar2", VariableType.Float).SetValue(true), "Assigning the wrong type value should throw an exception.");
        }

        [Test]
        public void TestBoolVariableAssignment()
        {
            _variablesContext.AddVariable("BoolVar1", VariableType.Boolean).SetValue(true);
            _variablesContext.AddVariable("FloatVar2", VariableType.Float).SetValue(0.03F);

            BoolTestClass testClass1 = new BoolTestClass();
            testClass1.Bool1 = false;
            testClass1.Bool3 = false;
            testClass1.Bool5 = false;

            bool testResult;
            Assert.IsFalse(testClass1.TestAssignVariable(_variablesContext, "Bool1", "BoolVar1", out string error), "TestAssignVariable should throw an exception for variable assignment on a non-variable parameter.");
            Assert.IsFalse(testClass1.TryAssignVariable(_variablesContext, "Bool1", "BoolVar1", out error), "TryAssignVariable should throw an exception for variable assignment on a non-variable parameter.");

            testResult = testClass1.TestAssignVariable(_variablesContext, "Bool2", "BoolVar1", out error);
            Assert.IsTrue(testResult, "TestAssignVariable should return true for valid variable assignment.");
            testResult = testClass1.TryAssignVariable(_variablesContext, "Bool2", "BoolVar1", out error);
            Assert.IsTrue(testResult, "TryAssignVariable should return true for valid variable assignment.");

            testResult = testClass1.TryAssignVariable(_variablesContext, "Bool3", "BoolVar1", out error);
            Assert.IsTrue(testResult, "TryAssignVariable should return true for valid variable assignment.");

            testResult = testClass1.TestAssignVariable(_variablesContext, "Bool2", "FloatVar2", out error);
            Assert.IsFalse(testResult, "TestSetParameter should return false for invalid variable type.");
            Assert.IsFalse(testClass1.TryAssignVariable(_variablesContext, "Bool2", "FloatVar2", out error), "TryAssignVariable should throw an exception for invalid variable type.");
        }

        [Test]
        public void TestBoolValidations()
        {
            _variablesContext.AddVariable("BoolVar1", VariableType.Boolean).SetValue(true);

            BoolTestClass testClass1 = new BoolTestClass();
            testClass1.Bool1 = false;
            testClass1.Bool3 = false;
            testClass1.Bool5 = false;

            bool testResult;

            testResult = testClass1.TryAssignVariable(_variablesContext, "Bool2", "BoolVar1", out string error);
            Assert.IsTrue(testResult, "TryAssignVariable should return true for valid variable assignment.");

            Dictionary<string, string> variableErrors = new Dictionary<string, string>();
            testResult = testClass1.ResolveVariables(_variablesContext, variableErrors);
            Assert.IsTrue(testResult, "ResolveVariables should return true for resolve variables.");
            Assert.That(0, Is.EqualTo(variableErrors.Count), "ResolveVariables should not have any errors.");

            Dictionary<string, List<string>> validateErrors = new Dictionary<string, List<string>>();
            testResult = testClass1.ValidateParameters(_variablesContext, validateErrors);

            _variablesContext.RemoveVariable("BoolVar1");
            testResult = testClass1.ResolveVariables(_variablesContext, variableErrors);
            Assert.IsFalse(testResult, "ResolveVariables should return false for resolve variables.");
            Assert.That(1, Is.EqualTo(variableErrors.Count), "ResolveVariables should have 1 errors.");
            Assert.IsTrue(variableErrors.Values.First().Contains("BoolVar1"), "ResolveVariables should have an error including the string 'BoolVar1'.");
            Assert.IsTrue(variableErrors.Keys.First().Contains("Bool2"), "ResolveVariables should have an error including the string 'BoolVar1'.");
        }
    }
}
