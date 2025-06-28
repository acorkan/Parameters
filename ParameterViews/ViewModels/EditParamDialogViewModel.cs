using CommunityToolkit.Mvvm.Input;
using ParameterModel.Interfaces;

namespace ParameterViews.ViewModels
{
    /// <summary>
    /// View model for the edit parameter dialog.
    /// </summary>
    public partial class EditParamDialogViewModel : ParamCollectionViewModel, ParameterViews.Interfaces.ICanCloseDialog    //: ViewModelBase, ICanCloseDialog
    {
        /// <summary>
        /// If set then the dialog is a new object and the OK option should always be set.
        /// </summary>
        private bool _isNew;
        //private Dictionary<PropertyInfo, ParameterPromptAttribute> _attributeMap;

        //[ObservableProperty]
        //private ObservableCollection<ViewModelBase> _parameters;

        public string Title { get; }
        
        [RelayCommand]
        private void Cancel()
        {
            AcceptChanges = false;
            CloseDialog?.Invoke();
        }

        [RelayCommand(CanExecute=nameof(OnOkCommandCanExecute))]
        private void Ok()
        {
            AcceptChanges = false;
            // Check if all parameters are valid, or changed.
            foreach (ParamViewModelNotifyBase param in Parameters)
            {
                if (param.IsDirty)
                {
                    AcceptChanges = true;
                    break;
                }
            }
            if(AcceptChanges)
            {
                foreach (ParamViewModelNotifyBase param in Parameters)
                {
                    if (!param.TryApplyChangedValue())
                    {
                        AcceptChanges = false;
                        break;
                    }
                }
            }
            CloseDialog?.Invoke();
        }

        public bool AcceptChanges { get; protected set; } = false;
        private bool OnOkCommandCanExecute()
        {
            if(_isNew)
            {
                return true;
            }
            bool isChanged = false;
            bool isValid = true;
            // Check if all parameters are valid, or changed.
            foreach (ParamViewModelNotifyBase param in Parameters)
            {
                if (!param.IsValid)
                {
                    isValid = false;
                }
                if (param.IsDirty)
                {
                    isChanged = true;
                }
            }
            return isChanged;
        }

        public EditParamDialogViewModel(string title, IImplementsParameterAttribute model, bool isNew, bool isReadOnly) :
            base(model, isReadOnly)
        {
            CanCloseDialog = true;
            Title = title;
            _isNew = isNew;
            //_attributeMap = ParameterPromptAttribute.GetAttributeMap(model);
            //Parameters = GetParamViewModelCollection(model, _attributeMap);
            //foreach (ViewModelBase param in Parameters)
            //{
            //    (param as ParamViewModelNotifyBase).OnUserInputChanged += ParamViewModel_OnPropertyChanged;
            //}
        }

        protected override void ParamViewModel_OnPropertyChanged(object sender, string propertyName)
        {
            base.ParamViewModel_OnPropertyChanged(sender, propertyName);
            OkCommand.NotifyCanExecuteChanged();
        }

        #region ICanCloseDialog
        public Action CloseDialog { get; set; }
        public bool CanCloseDialog { get; protected set; }
        #endregion ICanCloseDialog

//        public static List<ViewModelBase> GetParamViewModelCollection(IImplementsParameterPrompt model, Dictionary<PropertyInfo, ParameterPromptAttribute> attributeMap)
//        {
//            List<ViewModelBase> ret = new List<ViewModelBase>();
//            foreach (KeyValuePair<PropertyInfo, ParameterPromptAttribute> kvp in attributeMap)
//            {
//                ParamViewModelNotifyBase paramViewModel = null;
//                // Get the property name and value
//                string typeLabel;
//                if (kvp.Key.PropertyType == typeof(string))
//                {
//                    typeLabel = "string";
//                    paramViewModel = new StringParamViewModel(kvp.Value, kvp.Key, model);
//                }
//                else if (kvp.Key.PropertyType == typeof(int))
//                {
//                    typeLabel = "int";
//                    paramViewModel = new IntParamViewModel(kvp.Value, kvp.Key, model, (int)kvp.Value.Min, (int)kvp.Value.Max);
//                }
//                else if (kvp.Key.PropertyType == typeof(float))
//                {
//                    typeLabel = "float";
//                    paramViewModel = new FloatParamViewModel(kvp.Value, kvp.Key, model, kvp.Value.Min, kvp.Value.Max);
//                }
//                else if (kvp.Key.PropertyType == typeof(bool))
//                {
//                    typeLabel = "bool";
//                    paramViewModel = new BoolParamViewModel(kvp.Value, kvp.Key, model);
//                }
//                else if (kvp.Key.PropertyType == typeof(string[]))
//                {
//                    typeLabel = "string[]";
//                    paramViewModel = new StrArrayParamViewModel(kvp.Value, kvp.Key, model);
//                }
//                else if (kvp.Key.PropertyType.IsEnum)
//                {
//                    typeLabel = "enum";
//                    paramViewModel = new EnumParamViewModel(kvp.Value, kvp.Key, model);
//                }
//                else
//                {
//                    throw new InvalidOperationException($"Unsupported property type {kvp.Key.PropertyType}, must be string, int, float, bool, or string[].");
//                }
//#if DEBUG
//                //paramViewModel.Prompt += $" ({typeLabel}{range})";
//#endif
//                // Add the view model to the list of parameters
//                ret.Add(paramViewModel);
//            }
//            return ret;
//        }
    }
}
