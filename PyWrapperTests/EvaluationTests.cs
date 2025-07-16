using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;

namespace PyWrapperTests
{
    [TestFixture]
    public class EvaluationTests
    {

        private IVariablesContext _variablesContext;
        private PythonWrapper.PyEval _pyEval;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _variablesContext = new VariablesContext();
            _pyEval = new PythonWrapper.PyEval();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _pyEval.Dispose();
        }


        [SetUp]
        public void Setup()
        {
            _variablesContext.ClearVariables();
        }

        internal class ExecutionContext : EvaluationContextBase
        {
            public ExecutionContext() : base(new VariablesContext(), false, false)
            {
            }
            public ExecutionContext(bool isDebug, bool isSimulation) : base(new VariablesContext(), isDebug, isSimulation)
            {
            }
        }

        [Test]
        public void StaticEvalTest()
        {
            string code = "3 * 4 + 5"; // Example Python code to evaluate
            // Act
            bool result = _pyEval.Eval(code, _variablesContext, out string eval, out string error);
            Assert.IsTrue(result, $"Python code evaluation failed: {error}.");
            Assert.AreEqual("17", eval, "The evaluation result is not as expected.");
        }

        [Test]
        public void EvalMissingVariableTest()
        {
            string code = "3 * X + 5"; // Example Python code to evaluate
            _variablesContext.AddVariable("Y", VariableType.Integer).SetValue(5);
            // Act
            bool result = _pyEval.Eval(code, _variablesContext, out string eval, out string error);
            Assert.IsFalse(result, $"Variable X does not exist.");
            // Assert
            // You can add assertions here based on the expected output or behavior
            //Assert.AreEqual().Pass("Python code evaluated successfully.");
        }

        [Test]
        public void EvalMathTest()
        {
            string code = "3 * X + 5"; // Example Python code to evaluate
            _variablesContext.AddVariable("X", VariableType.Integer).SetValue(5);
            bool result = _pyEval.Eval(code, _variablesContext, out string eval, out string error);
            Assert.IsTrue(result, $"Python code evaluation failed: {error}.");
            Assert.AreEqual("20", eval, "The evaluation result is not as expected.");
        }

        [Test]
        public void EvalBoolTest()
        {
            string code = "X == 5"; // Example Python code to evaluate
            _variablesContext.AddVariable("X", VariableType.Integer).SetValue(5);
            bool result = _pyEval.Eval(code, _variablesContext, out string eval, out string error);
            Assert.IsTrue(result, $"Python code evaluation failed: {error}.");
            Assert.AreEqual("True", eval, "The evaluation result is not as expected.");

            _variablesContext.GetVariable("X").SetValue(6);
            result = _pyEval.Eval(code, _variablesContext, out eval, out error);
            Assert.IsTrue(result, $"Python code evaluation failed: {error}.");
            Assert.AreEqual("False", eval, "The evaluation result is not as expected.");
        }
    }
}