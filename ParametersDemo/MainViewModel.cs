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

            _variablesContext.AddVariable("intVar1", VariableType.Integer).SetValue(1);
            _variablesContext.AddVariable("intVar2", VariableType.Integer).SetValue(20);
            _variablesContext.AddVariable("intVar3", VariableType.Integer).SetValue(300);

            _variablesContext.AddVariable("floatVar1", VariableType.Float).SetValue(1.1F);
            _variablesContext.AddVariable("floatVar2", VariableType.Float).SetValue(2.2F);
            _variablesContext.AddVariable("floatVar3", VariableType.Float).SetValue(3.3F);

            _variablesContext.AddVariable("stringVar3", VariableType.String).SetValue("foo");
            _variablesContext.AddVariable("stringVar3", VariableType.String).SetValue("Boo");
            _variablesContext.AddVariable("stringVar3", VariableType.String).SetValue("Bar");
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
            EnumTestClass paramTestClass = new EnumTestClass();
            Prompt("Command 1 Prompt", paramTestClass);
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
