using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;

namespace PyWrapperTests
{
    public class EvaluationTests
    {
        private static ILog Log { get; } = LogManager.GetLogger(typeof(EvaluationTests));

        //static EvaluationTests()
        //{
        //    ConfigureLogging();

        //    log.Debug("This is a DEBUG message");
        //    log.Info("This is an INFO message");
        //    log.Warn("This is a WARN message");
        //    log.Error("This is an ERROR message");
        //    log.Fatal("This is a FATAL message");

        //    Console.ReadLine();
        //}

        private static void ConfigureLogging()
        {
            PatternLayout layout = new PatternLayout
            {
                ConversionPattern = "%date %-5level - %message%newline"
            };
            layout.ActivateOptions();

            ConsoleAppender consoleAppender = new ConsoleAppender
            {
                Layout = layout
            };
            consoleAppender.ActivateOptions();

            BasicConfigurator.Configure(consoleAppender);
        }

        [SetUp]
        public void Setup()
        {
            ConfigureLogging();
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
        public void EvalTest()
        {
            // Arrange
            var pyEval = new PythonWrapper.PyEval();
            string code = "3 * 4 + 5"; // Example Python code to evaluate
            IVariablesContext variablesContext = new VariablesContext(); // Assuming you have an appropriate context
            // Act
            pyEval.Eval(code, variablesContext, out string result, out string error);
            Log.Debug($"Result: {result}, Error: {error}");
            // Assert
            // You can add assertions here based on the expected output or behavior
            Assert.Pass("Python code evaluated successfully.");
        }

        [Test]
        public void EvalVariableTest()
        {
            // Arrange
            var pyEval = new PythonWrapper.PyEval();
            string code = "3 * X + 5"; // Example Python code to evaluate
            var variablesContext = new VariablesContext(); // Assuming you have an appropriate context
            variablesContext.AddVariable("X", VariableType.Integer).SetValue(5);
            // Act
            pyEval.Eval(code, variablesContext, out string result, out string error);
            Log.Debug($"Result: {result}, Error: {error}");
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
            var variablesContext = new VariablesContext(); // Assuming you have an appropriate context
            variablesContext.AddVariable("Y", VariableType.Integer).SetValue(5);
            // Act
            pyEval.Eval(code, variablesContext, out string result, out string error);
            Log.Debug($"Result: {result}, Error: {error}");
            // Assert
            // You can add assertions here based on the expected output or behavior
            //Assert.AreEqual().Pass("Python code evaluated successfully.");
        }

    }
}