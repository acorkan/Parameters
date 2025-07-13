namespace ParameterModel.Interfaces
{
    public interface IEvaluationContext
    {
        //ILog Log { get; }
        bool IsDebug { get; }
        bool IsSimulation { get; }

        IVariablesContext VariablesContext { get; }
    }
}
