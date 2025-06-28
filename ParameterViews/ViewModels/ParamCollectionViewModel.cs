using CommunityToolkit.Mvvm.ComponentModel;
using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ParameterViews.ViewModels
{
    /// <summary>
    /// View model for a collection of parameters that are implementations of ParamViewModelBase<T>.
    /// </summary>
    public partial class ParamCollectionViewModel : ViewModelBase
    {
        protected Dictionary<PropertyInfo, ParameterAttribute> _attributeMap;
        private bool _isReadOnly;

        protected Dictionary<string, Func<IImplementsParameterAttribute, string>> _globalValidationMap;

        [ObservableProperty]
        private ObservableCollection<ViewModelBase> _parameters;

        public ParamCollectionViewModel(IImplementsParameterAttribute model, bool isReadOnly)
        {
            _isReadOnly = isReadOnly;
            _attributeMap = ParameterAttribute.GetAttributeMap(model);
            Parameters = new ObservableCollection<ViewModelBase>(GetParamViewModelCollection(model, _attributeMap));
            foreach (ViewModelBase param in Parameters)
            {
                (param as ParamViewModelNotifyBase).OnUserInputChanged += ParamViewModel_OnPropertyChanged;
                (param as ParamViewModelNotifyBase).IsReadOnly = isReadOnly;
            }
        }

        protected virtual void ParamViewModel_OnPropertyChanged(object sender, string propertyName)
        {
            System.Diagnostics.Trace.WriteLine($"Property changed: {propertyName}");
        }

        protected static List<ViewModelBase> GetParamViewModelCollection(IImplementsParameterAttribute model, Dictionary<PropertyInfo, ParameterAttribute> attributeMap)
        {
            List<ViewModelBase> ret = new List<ViewModelBase>();
            foreach (KeyValuePair<PropertyInfo, ParameterAttribute> kvp in attributeMap)
            {
                ParamViewModelNotifyBase paramViewModel = null;
                // Get the property name and value
                string typeLabel;
                if (kvp.Key.PropertyType == typeof(string))
                {
                    typeLabel = "string";
                    paramViewModel = new StringParamViewModel(kvp.Value, kvp.Key, model);
                }
                else if (kvp.Key.PropertyType == typeof(int))
                {
                    typeLabel = "int";
                    paramViewModel = new IntParamViewModel(kvp.Value, kvp.Key, model, (int)kvp.Value.Min, (int)kvp.Value.Max);
                }
                else if (kvp.Key.PropertyType == typeof(float))
                {
                    typeLabel = "float";
                    paramViewModel = new FloatParamViewModel(kvp.Value, kvp.Key, model, kvp.Value.Min, kvp.Value.Max);
                }
                else if (kvp.Key.PropertyType == typeof(bool))
                {
                    typeLabel = "bool";
                    paramViewModel = new BoolParamViewModel(kvp.Value, kvp.Key, model);
                }
                else if (kvp.Key.PropertyType == typeof(string[]))
                {
                    typeLabel = "string[]";
                    paramViewModel = new StrArrayParamViewModel(kvp.Value, kvp.Key, model);
                }
                else if (kvp.Key.PropertyType.IsEnum)
                {
                    typeLabel = "enum";
                    paramViewModel = new EnumParamViewModel(kvp.Value, kvp.Key, model);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported property type {kvp.Key.PropertyType}, must be string, int, float, bool, or string[].");
                }
#if DEBUG
                //paramViewModel.Prompt += $" ({typeLabel}{range})";
#endif
                // Add the view model to the list of parameters
                ret.Add(paramViewModel);
            }
            return ret;
        }

    }
}
