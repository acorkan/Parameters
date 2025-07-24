using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Factories;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
using ParameterViews.Factories;
using ParameterViews.ViewModels;
using System;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ParametersDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public class TestBoolean : ImplementsParametersBase
        {
            [Parameter]
            public bool Bool1 { get; set; } = true;
            [Parameter(true)]
            public bool Bool2 { get; set; } = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ViewModelBase.TraceMessagesOn = true;

            IVariablesContext _variablesContext = new VariablesContext();

            ParameterModelFactory modelFactory = new ParameterModelFactory();

            ParameterViewModelFactory viewModelFactory = new ParameterViewModelFactory(modelFactory);

            List<ParamViewModelBase> parameters = viewModelFactory.GetParameterViewModels(new TestBoolean(), _variablesContext, true);

            //ParameterModels parameterModels = new ParameterModels();
            // Create parameter class.
            // Process throught factory to get prompt models.
            // Create promt view models for each parameter using factory. This becomes List<ParamViewModelBase> parameters
            MainViewModel mainViewModel = new MainViewModel(parameters, false); // List<ParamViewModelBase> parameters, bool isReadOnly
            MainWindow mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };

            MainWindow.Show();
        }
    }

}
