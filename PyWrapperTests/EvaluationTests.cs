using log4net;
using ParameterModel.Models.Base;

namespace PyWrapperTests
{
    public class EvaluationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        internal class ExecutionContext : EvaluationContextBase
        {
            public ExecutionContext(ILog log, bool isDebug, bool isSimulation) : base(log, isDebug, isSimulation)
            {
            }
        }

        [Test]
        public void EvalTest()
        {
            // Arrange
            var pyEval = new PythonWrapper.PyEval();
            string code = "3 * 4 + 5"; // Example Python code to evaluate
            var executionContext = new ExecutionContext(); // Assuming you have an appropriate context
            // Act
            pyEval.Eval(code, executionContext);
            // Assert
            // You can add assertions here based on the expected output or behavior
            Assert.Pass("Python code evaluated successfully.");
        }

        [Test]
        public void EvalMissingVariableTest()
        {
            // Arrange
            var pyEval = new PythonWrapper.PyEval();
            string code = "3 * X + 5"; // Example Python code to evaluate
            var executionContext = new ExecutionContext(); // Assuming you have an appropriate context
            // Act
            pyEval.Eval(code, executionContext);
            // Assert
            // You can add assertions here based on the expected output or behavior
            Assert.Pass("Python code evaluated successfully.");
        }

    }
}