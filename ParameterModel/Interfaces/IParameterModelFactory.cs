namespace ParameterModel.Interfaces
{
    public interface IParameterModelFactory
    {
        Dictionary<string, IParameterModel> GetModels(IImplementsParameterAttribute propertyOwner);
    }
}
