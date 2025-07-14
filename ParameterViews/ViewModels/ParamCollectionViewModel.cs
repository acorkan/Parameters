using CommunityToolkit.Mvvm.ComponentModel;
using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ParameterViews.ViewModels
{
    /// <summary>
    /// View model for a collection of parameters that are implementations of ParamViewModelBase<T>.
    /// </summary>
    public partial class ParamPromptViewModel : ViewModelBase<ParameterModelMessage>
    {
        private bool _isReadOnly;

        [ObservableProperty]
        private ObservableCollection<ParamViewModelBase> _parameters;

        public ParamPromptViewModel(List<ParamViewModelBase> parameters, bool isReadOnly)
        {
            _parameters = new ObservableCollection<ParamViewModelBase>(parameters);
            _isReadOnly = isReadOnly;
            foreach (ParamViewModelBase param in Parameters)
            {
            //    param as ParamViewModelBase).OnUserInputChanged += ParamViewModel_OnPropertyChanged;
                (param as ParamViewModelBase).IsReadOnly = isReadOnly;
            }
        }

        protected virtual void ParamViewModel_OnPropertyChanged(object sender, string propertyName)
        {
            System.Diagnostics.Trace.WriteLine($"Property changed: {propertyName}");
        }
    }
}
