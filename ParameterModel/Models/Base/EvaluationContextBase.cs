using log4net;
using ParameterModel.Interfaces;

namespace ParameterModel.Models.Base
{
    public class EvaluationContextBase : IEvaluationContext
    {
        public ILog Log { get; private set; }
        public bool IsDebug { get; private set; }
        public bool IsSimulation { get; private set; }

        public Dictionary<string, string> Variables { get; } = new Dictionary<string, string>();

        public EvaluationContextBase(ILog log, bool isDebug, bool isSimulation)
        {
            //Log = log ?? throw new ArgumentNullException(nameof(log));
            IsDebug = isDebug;
            IsSimulation = isSimulation;
        }
        // Implement other members of IVariablesContext if needed
    }
}
