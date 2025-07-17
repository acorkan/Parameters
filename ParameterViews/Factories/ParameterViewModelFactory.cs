using ParameterModel.Factories;
using ParameterModel.Interfaces;
using ParameterModel.Models;
using ParameterModel.Variables;
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
            Dictionary<string, IParameterModel> models = _parameterModelFactory.GetModels(propertyOwner);

            List<ParamViewModelBase> ret = new List<ParamViewModelBase>();
            foreach (KeyValuePair<string, IParameterModel> kvp in models)
            {
                ParamViewModelBase paramViewModel = null;
                // Get the property name and value
                if (kvp.Value.ParameterType == typeof(string))
                {
                    paramViewModel = new StringParamViewModel(kvp.Value as StringParameterModel);
                }
                else if (kvp.Value.ParameterType == typeof(int))
                {
                    paramViewModel = new IntParamViewModel(kvp.Value as IntParameterModel);
                }
                else if (kvp.Value.ParameterType == typeof(float))
                {
                    paramViewModel = new FloatParamViewModel(kvp.Value as FloatParameterModel);
                }
                else if (kvp.Value.ParameterType == typeof(bool))
                {
                    paramViewModel = new BoolParamViewModel(kvp.Value as BoolParameterModel);
                }
                else if (kvp.Value.ParameterType == typeof(string[]))
                {
                    paramViewModel = new StrArrayParamViewModel(kvp.Value as StringArrayParameterModel);
                }
                else if (kvp.Value.ParameterType == typeof(Variable))
                {
                    paramViewModel = new VariableParamViewModel(kvp.Value as VariableParameterModel);
                }
                else if (kvp.Value.ParameterType.IsEnum)
                {
                    paramViewModel = new EnumParamViewModel(kvp.Value as EnumParameterModel);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported property type {kvp.Value.ParameterType}.");
                }
                ret.Add(paramViewModel);
            }
            return ret;
        }

        /// <summary>
        /// Build a ViewModel for the given property owner that implements IImplementsParameterAttribute.
        /// Use this VM for display or prompt dialogs
        /// </summary>
        /// <param name="propertyOwner"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        public ParamPromptViewModel GetParamPromptViewModel(IImplementsParameterAttribute propertyOwner, bool isReadOnly)
        {
            return new ParamPromptViewModel(GetParameterViewModels(propertyOwner), isReadOnly);
        }

        public EditParamDialogViewModel GetParamCollectionViewModel(string title, IImplementsParameterAttribute propertyOwner, bool isReadOnly, bool isNew)
        {
            return new EditParamDialogViewModel(title, GetParameterViewModels(propertyOwner), isReadOnly, isNew);
        }
    }
}
