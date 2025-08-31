namespace ParameterModel.Interfaces
{
    public interface IEvaluationContext
    {
        //ILog Log { get; }
        bool IsDebug { get; }
        bool IsSimulation { get; }

        IVariablesContext VariablesContext { get; }

        bool Eval(string code, out string result, out string error);

        bool EvalInt(string code, out int result, out string error);

        bool EvalFloat(string code, out float result, out string error);

        bool EvalBool(string code, out bool result, out string error);

        bool EvalJson(string code, out string result, out string error);
    }
}
