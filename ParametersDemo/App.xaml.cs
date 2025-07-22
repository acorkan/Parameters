using MileHighWpf.MvvmModelMessaging;
using ParameterModel.Attributes;
using ParameterModel.Interfaces;
using ParameterModel.Models.Base;
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

            //ParameterModels parameterModels = new ParameterModels();
            // Create parameter class.
            // Process throught factory to get prompt models.
            // Create promt view models for each parameter using factory. This becomes List<ParamViewModelBase> parameters
            MainViewModel mainViewModel = new MainViewModel(parameterModels); // List<ParamViewModelBase> parameters, bool isReadOnly
            MainWindow mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };

        }
    }

}
