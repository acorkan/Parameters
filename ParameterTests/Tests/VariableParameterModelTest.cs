using ParameterModel.Extensions;
using ParameterModel.Interfaces;
using ParameterModel.Variables;
using ParameterTests.TestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParameterTests.Tests
{
    [TestFixture]
    internal class VariableParameterModelTest : BoolParameterModelTests
    {
        [Test]
        public void TestVarSerializeParameters()
        {
            VariableParamTestClass testClass1 = new VariableParamTestClass();
            testClass1.Bool1 = !testClass1.Bool1;
            testClass1.Bool4 = !testClass1.Bool4;
            testClass1.Bool5 = !testClass1.Bool5;

            testClass1.Var1 = null;
            testClass1.Var5 = new VariableProperty("IntVar1");

            string json = testClass1.SerializeParametersToJson();

            VariableParamTestClass testClass2 = new VariableParamTestClass();
            testClass2.UpdateParametersFromJson<VariableParamTestClass>(json);

            Assert.AreEqual(testClass1.Bool1, testClass2.Bool1, "Bool1 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool2, testClass2.Bool2, "Bool2 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool3, testClass2.Bool3, "Bool3 should be equal after serialization and deserialization.");
            Assert.AreNotEqual(testClass1.Bool4, testClass2.Bool4, "Bool4 should not be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool5, testClass2.Bool5, "Bool5 should be equal after serialization and deserialization.");

            Assert.AreEqual(testClass1.Var1, testClass2.Var1, "Var1 should not be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Var5.Assignment, testClass2.Var5.Assignment, "Bool5 should be equal after serialization and deserialization.");

            //Assert.AreEqual(testClass1.GetAssignedVariable("Bool3"), "BoolVar2", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            //Assert.AreEqual(testClass1.GetAssignedVariable("Bool2"), "BoolVar1", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            //Assert.AreEqual(testClass2.GetAssignedVariable("Bool3"), "BoolVar2", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            //Assert.AreEqual(testClass2.GetAssignedVariable("Bool2"), "BoolVar1", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
        }

        [Test]
        public void TestBoolAndVarSerializeParametersWithVariables()
        {
            _variablesContext.AddVariable("BoolVar1", VariableType.Boolean).SetValue(true);
            _variablesContext.AddVariable("BoolVar2", VariableType.Boolean).SetValue(false);

            VariableParamTestClass testClass1 = new VariableParamTestClass();

            bool testResult;
            testClass1.TryAssignVariable(_variablesContext, "Bool3", "BoolVar2", out string error);
            testClass1.TryAssignVariable(_variablesContext, "Bool2", "BoolVar1", out error);

            string json = testClass1.SerializeParametersToJson();

            VariableParamTestClass testClass2 = new VariableParamTestClass();
            testClass2.UpdateParametersFromJson<VariableParamTestClass>(json);
            Assert.That("BoolVar2", Is.EqualTo(testClass1.VariableAssignments["Bool3"]));
            Assert.That("BoolVar1", Is.EqualTo(testClass1.VariableAssignments["Bool2"]));
            Assert.That("BoolVar2", Is.EqualTo(testClass2.VariableAssignments["Bool3"]));
            Assert.That("BoolVar1", Is.EqualTo(testClass2.VariableAssignments["Bool2"]));
        }

        [Test]
        public void TestBoolAndVarGenericSerialize()
        {
            _variablesContext.AddVariable("BoolVar1", VariableType.Boolean).SetValue(true);
            _variablesContext.AddVariable("BoolVar2", VariableType.Boolean).SetValue(false);

            VariableParamTestClass testClass1 = new VariableParamTestClass();
            testClass1.Bool1 = !testClass1.Bool1;
            testClass1.Bool4 = !testClass1.Bool4;
            testClass1.Bool5 = !testClass1.Bool5;

            testClass1.Var1 = null;
            testClass1.Var5 = new VariableProperty("IntVar1");

            bool testResult;
            testClass1.TryAssignVariable(_variablesContext, "Bool3", "BoolVar2", out string error);
            testClass1.TryAssignVariable(_variablesContext, "Bool2", "BoolVar1", out error);

            string json = JsonSerializer.Serialize(testClass1);

            VariableParamTestClass testClass2 = JsonSerializer.Deserialize<VariableParamTestClass>(json);

            Assert.AreEqual(testClass1.Bool1, testClass2.Bool1, "Bool1 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool2, testClass2.Bool2, "Bool2 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool3, testClass2.Bool3, "Bool3 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool4, testClass2.Bool4, "Bool4 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Bool5, testClass2.Bool5, "Bool5 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.GetAssignedVariable("Bool3"), "BoolVar2", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            Assert.AreEqual(testClass1.GetAssignedVariable("Bool2"), "BoolVar1", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            Assert.AreEqual(testClass2.GetAssignedVariable("Bool3"), "BoolVar2", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            Assert.AreEqual(testClass2.GetAssignedVariable("Bool2"), "BoolVar1", "Bool3 should have the assigned variable BoolVar2 after serialization and deserialization.");
            Assert.AreEqual(testClass1.Var1, testClass2.Var1, "Var1 should not be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Var5.Assignment, testClass2.Var5.Assignment, "Bool5 should be equal after serialization and deserialization.");
        }
    }
}
