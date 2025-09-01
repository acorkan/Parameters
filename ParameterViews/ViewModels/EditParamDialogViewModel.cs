using CommunityToolkit.Mvvm.Input;
using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Interfaces;
using System.Collections.Generic;

namespace ParameterViews.ViewModels
{
    /// <summary>
    /// View model for the edit parameter dialog.
    /// </summary>
    public partial class EditParamDialogViewModel : ParamPromptViewModel, ParameterViews.Interfaces.ICanCloseDialog
    {
        private readonly List<ParamViewModelBase> _parameters;
        /// <summary>
        /// If set then the dialog is a new object and the OK option should always be set.
        /// </summary>
        private bool _isNew;

        public string Title { get; }

        public string Caption { get; }

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
            foreach (ParamViewModelBase param in Parameters)
            {
                //if (param.IsDirty)
                //{
                //    AcceptChanges = true;
                //    break;
                //}
            }
            if(AcceptChanges)
            {
                foreach (ParamViewModelBase param in Parameters)
                {
                    //if (!param.TryApplyChangedValue())
                    //{
                    //    AcceptChanges = false;
                    //    break;
                    //}
                }
            }
            foreach (ParamViewModelBase param in _parameters)
            {
                param.PropertyChanged -= Param_PropertyChanged;
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
            foreach (ParamViewModelBase param in Parameters)
            {
                if (param.IsError)
                {
                    isValid = false;
                }
                if (param.IsModified)
                {
                    isChanged = true;
                }
            }
            return isChanged && isValid;
        }

        public EditParamDialogViewModel(string title, List<ParamViewModelBase> parameters, bool isReadOnly, bool isNew, string caption = null) :
            base(parameters, isReadOnly)
        {
            CanCloseDialog = true;
            Title = title;
            Caption = caption;
            _isNew = isNew;
            //_attributeMap = ParameterPromptAttribute.GetAttributeMap(model);
            //Parameters = GetParamViewModelCollection(model, _attributeMap);
            _parameters = parameters;
            foreach (ParamViewModelBase param in _parameters)
            {
                param.PropertyChanged += Param_PropertyChanged;  // += ParamViewModel_OnPropertyChanged;
            }
            if (isReadOnly)
            {
                Title += " (ReadOnly)"; 
            }
        }

        private void Param_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.ParamViewModel_OnPropertyChanged(sender, e.PropertyName);
            OkCommand.NotifyCanExecuteChanged();
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
    }
}
