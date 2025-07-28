using ParameterModel.Extensions;
using ParameterModel.Factories;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using ParameterTests.TestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterTests.Tests
{
    [TestFixture]
    internal class IntAndFloatParameterModelTests
    {
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

        [Test]
        public void TestIntAndFloatSerialize()
        {
            IntAndFloatTestClass testClass1 = new IntAndFloatTestClass();
            testClass1.Int1++;
            testClass1.Int2++;
            testClass1.Int3++;
            testClass1.Int4++;
            testClass1.Int5++;

            testClass1.Float1++;
            testClass1.Float2++;
            testClass1.Float3++;
            testClass1.Float4++;
            testClass1.Float5++;

            string json = testClass1.SerializeParametersToJson();

            IntAndFloatTestClass testClass2 = new IntAndFloatTestClass();
            testClass2.UpdateParametersFromJson<IntAndFloatTestClass>(json);

            Assert.AreEqual(testClass1.Int1, testClass2.Int1, "TestInt1 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Int2, testClass2.Int2, "TestInt2 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Int3, testClass2.Int3, "TestInt3 should be equal after serialization and deserialization.");
            Assert.AreNotEqual(testClass1.Int4, testClass2.Int4, "TestInt4 should not be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Int5, testClass2.Int5, "TestInt5 should be equal after serialization and deserialization.");

            Assert.AreEqual(testClass1.Float1, testClass2.Float1, "TestFloat1 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Float2, testClass2.Float2, "TestFloat2 should be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Float3, testClass2.Float3, "TestFloat3 should be equal after serialization and deserialization.");
            Assert.AreNotEqual(testClass1.Float4, testClass2.Float4, "TestFloat4 should not be equal after serialization and deserialization.");
            Assert.AreEqual(testClass1.Float5, testClass2.Float5, "TestFloat5 should be equal after serialization and deserialization.");
        }

        [Test]
        public void TestIntAndFloatRangeAttributeOk()
        {
            IntAndFloatTestClass testClass1 = 
                new IntAndFloatTestClass();
            testClass1.SerializeParametersToJson();
            Assert.DoesNotThrow(() => _parameterModelFactory.GetModels(testClass1), "No Range exception for int or float property.");
        }

        [Test]
        public void TestValidateIntAndFloatInitialValue()
        {
            IntAndFloatTestClass testClass1 = new IntAndFloatTestClass();
            Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
            Assert.IsFalse(testClass1.ValidateParameters(null, errors));
            Assert.IsTrue(errors.Count == 2);
            Assert.IsTrue(errors.ContainsKey("Int1"));
            Assert.IsTrue(errors.ContainsKey("Float5"));
        }

    }
}
