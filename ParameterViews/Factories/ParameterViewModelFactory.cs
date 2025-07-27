using ParameterModel.Factories;
using ParameterModel.Interfaces;
using ParameterModel.Models;
using ParameterModel.Models.Base;
using ParameterModel.Variables;
using ParameterViews.ViewModels;
using ParameterViews.ViewModels.Prompts;

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
        /// The showPrompt property controls if the left side test is shown or not. Typically for a dialog prompt it would be.
        /// </summary>
        /// <param name="propertyOwner"></param>
        /// <param name="showPrompt"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public List<ParamViewModelBase> GetParameterViewModels(IImplementsParameterAttribute propertyOwner, 
            IVariablesContext variablesContext, bool showPrompt = true)
        {
            Dictionary<string, IParameterModel> models = _parameterModelFactory.GetModels(propertyOwner);

            List<ParamViewModelBase> ret = new List<ParamViewModelBase>();
            foreach (KeyValuePair<string, IParameterModel> kvp in models)
            {
                ParamViewModelBase paramViewModel = null;
                // Get the property name and value
                if (kvp.Value.ParameterType == typeof(string))
                {
                    paramViewModel = new StringParamViewModel(kvp.Value as StringParameterModel, variablesContext, showPrompt);
                }
                else if (kvp.Value.ParameterType == typeof(int))
                {
                    paramViewModel = new IntParamViewModel(kvp.Value as IntParameterModel, variablesContext, showPrompt);
                }
                else if (kvp.Value.ParameterType == typeof(float))
                {
                    paramViewModel = new FloatParamViewModel(kvp.Value as FloatParameterModel, variablesContext, showPrompt);
                }
                else if (kvp.Value.ParameterType == typeof(bool))
                {
                    paramViewModel = new BoolParamViewModel(kvp.Value as BoolParameterModel, variablesContext, showPrompt);
                }
                else if (kvp.Value.ParameterType == typeof(string[]))
                {
                    paramViewModel = new StrArrayParamViewModel(kvp.Value as StringArrayParameterModel, variablesContext, showPrompt);
                }
                else if (kvp.Value.ParameterType == typeof(VariableProperty))
                {
                    paramViewModel = new VariableParamViewModel(kvp.Value as VariableParameterModel, variablesContext, showPrompt);
                }
                else if (kvp.Value.ParameterType.IsEnum)
                {
                    paramViewModel = new EnumParamViewModel(kvp.Value as EnumParameterModel, variablesContext, showPrompt);
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
        public ParamPromptViewModel GetParamPromptViewModel(IImplementsParameterAttribute propertyOwner, IVariablesContext variablesContext, bool isReadOnly)
        {
            return new ParamPromptViewModel(GetParameterViewModels(propertyOwner, variablesContext), isReadOnly);
        }

        public EditParamDialogViewModel GetParamCollectionViewModel(string title, IImplementsParameterAttribute propertyOwner, IVariablesContext variablesContext, bool isReadOnly, bool isNew)
        {
            return new EditParamDialogViewModel(title, GetParameterViewModels(propertyOwner, variablesContext), isReadOnly, isNew);
        }
    }
}
