using CommunityToolkit.Mvvm.Input;
using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Factories;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using ParameterTests.TestClasses;
using ParameterViews.Dialogs;
using ParameterViews.Factories;
using ParameterViews.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParametersDemo
{
    public partial class MainViewModel : ViewModelBase// ParamPromptViewModel

    {
        private IVariablesContext _variablesContext;
        private ParameterViewModelFactory _parameterViewModelFactory = 
            new ParameterViewModelFactory(new ParameterModelFactory());

        public MainViewModel(List<ParamViewModelBase> parameters, bool isReadOnly) //: base(parameters, isReadOnly)
        {
            _variablesContext = new VariablesContext();

            _variablesContext.AddVariable("trueVar", VariableType.Boolean).SetValue(true);
            _variablesContext.AddVariable("falseVar", VariableType.Boolean).SetValue(false);
        }

        private bool Prompt(string title, IImplementsParameterAttribute implements)
        {
            List<ParamViewModelBase> vms = _parameterViewModelFactory.GetParameterViewModels(implements,
                _variablesContext, true);
            EditParamDialogViewModel promptVM = new EditParamDialogViewModel(title, vms, false, false);
            EditLabwareDialog dlg = new EditLabwareDialog();
            dlg.DataContext = promptVM;
            bool? ret = dlg.ShowDialog();
            return promptVM.AcceptChanges;
        }
        [RelayCommand]
        public void Prompt1()
        {
            VariableParamTestClass variableParamTestClass = new VariableParamTestClass();
            Prompt("Command 1 Prompt", variableParamTestClass);
        }

        [RelayCommand]
        public void Prompt2()
        {
            IntAndFloatTestClass paramTestClass = new IntAndFloatTestClass();
            Prompt("Command 1 Prompt", paramTestClass);
        }

        [RelayCommand]
        public void Prompt3()
        {
        }

        [RelayCommand]
        public void Prompt4()
        {
        }

        [RelayCommand]
        public void Prompt5()
        {
        }

    }
}
