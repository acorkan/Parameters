namespace ParameterModel.Interfaces
{
    public interface IStatementEvaluator
    {
        bool TryEvaluate(string expression, out string result, List<string> errors);
    }
}
