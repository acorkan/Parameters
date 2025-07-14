using ParameterModel.Factories;
using ParameterModel.Interfaces;
using ParameterModel.Models;
using ParameterViews.ViewModels;

namespace ParameterViews.Factories
{
    public class ParameterViewModelFactory
    {
        private readonly ParameterModelFactory _parameterModelFactory;

        public ParameterViewModelFactory(ParameterModelFactory parameterModelFactory)
        {
            _parameterModelFactory = parameterModelFactory;
        }

        /// <summary>
        /// Build a collection of ViewModels from the property owner that implements IImplementsParameterAttribute.
        /// </summary>
        /// <param name="propertyOwner"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public List<ParamViewModelBase> GetParameterViewModels(IImplementsParameterAttribute propertyOwner)
        {
            Dictionary<string, IParameterModel> models = _parameterModelFactory.GetModelsCollection(propertyOwner);

            List<ParamViewModelBase> ret = new List<ParamViewModelBase>();
            foreach (KeyValuePair<string, IParameterModel> kvp in models)
            {
                ParamViewModelBase paramViewModel = null;
                // Get the property name and value
                string typeLabel;
                if (kvp.Value.ParameterType == typeof(string))
                {
                    typeLabel = "string";
                    paramViewModel = new StringParamViewModel(kvp.Value as StringParameterModel);
                }
                else if (kvp.Value.ParameterType == typeof(int))
                {
                    typeLabel = "int";
                    paramViewModel = new IntParamViewModel(kvp.Value as IntParameterModel);
                }
                else if (kvp.Value.ParameterType == typeof(float))
                {
                    typeLabel = "float";
                    paramViewModel = new FloatParamViewModel(kvp.Value as FloatParameterModel);
                }
                else if (kvp.Value.ParameterType == typeof(bool))
                {
                    typeLabel = "bool";
                    paramViewModel = new BoolParamViewModel(kvp.Value as BoolParameterModel);
                }
                else if (kvp.Value.ParameterType == typeof(string[]))
                {
                    typeLabel = "string[]";
                    paramViewModel = new StrArrayParamViewModel(kvp.Value as StringArrayParameterModel);
                }
                else if (kvp.Value.ParameterType.IsEnum)
                {
                    typeLabel = "enum";
                    paramViewModel = new EnumParamViewModel(kvp.Value as EnumParameterModel);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported property type {kvp.Value.ParameterType}, must be string, int, float, bool, or string[].");
                }
                ret.Add(paramViewModel);
            }
            return ret;
        }

        /// <summary>
        /// Build a viewModel collection for the given property owner that implements IImplementsParameterAttribute.
        /// </summary>
        /// <param name="propertyOwner"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        public ParamCollectionViewModel GetParamCollectionViewModel(IImplementsParameterAttribute propertyOwner, bool isReadOnly)
        {
            return new ParamCollectionViewModel(GetParameterViewModels(propertyOwner), isReadOnly);
        }
    }
}
